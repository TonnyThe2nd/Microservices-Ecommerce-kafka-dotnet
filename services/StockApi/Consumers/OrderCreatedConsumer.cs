using Confluent.Kafka;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class OrderCreatedConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _configuration;

    public OrderCreatedConsumer(
        IServiceScopeFactory scopeFactory,
        IConfiguration configuration)
    {
        _scopeFactory = scopeFactory;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _configuration["Kafka:BootstrapServers"],
            GroupId = "stock-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer =
            new ConsumerBuilder<string, string>(config)
                .Build();

        consumer.Subscribe("order-created");

        Console.WriteLine("📦 Stock Consumer iniciado");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var result = consumer.Consume(stoppingToken);

                var order =
                    JsonSerializer.Deserialize<OrderCreatedEvent>(
                        result.Message.Value);

                if (order == null)
                    continue;

                Console.WriteLine(
                    $"📥 Pedido {order.OrderId} recebido");

                using var scope =
                    _scopeFactory.CreateScope();

                var stockService =
                    scope.ServiceProvider
                        .GetRequiredService<StockService>();

                var producer =
                    scope.ServiceProvider
                        .GetRequiredService<StockEventProducer>();

                var dbContext =
                    scope.ServiceProvider
                        .GetRequiredService<StockDbContext>();

                await using var transaction =
                    await dbContext.Database
                        .BeginTransactionAsync(stoppingToken);

                bool allSuccess = true;

                foreach (var item in order.Items)
                {
                    bool reserved =
                        await stockService.ReserveStockAsync(
                            item.ProductId,
                            item.Quantity);

                    if (!reserved)
                    {
                        allSuccess = false;

                        Console.WriteLine(
                            $"❌ Estoque insuficiente para produto {item.ProductId}");

                        break;
                    }
                }

                if (!allSuccess)
                {
                    await transaction.RollbackAsync(
                        stoppingToken);

                    await producer.PublishAsync(
                        new StockProcessedEvent
                        {
                            OrderId = order.OrderId,
                            Success = false,
                            Message = "Out of stock",
                            ProcessedAt = DateTime.UtcNow
                        });

                    continue;
                }

                await stockService.SaveChangesAsync();

                await transaction.CommitAsync(
                    stoppingToken);

                await producer.PublishAsync(
                    new StockProcessedEvent
                    {
                        OrderId = order.OrderId,
                        Success = true,
                        Message = "Stock reserved",
                        ProcessedAt = DateTime.UtcNow
                    });

                Console.WriteLine(
                    $"✅ Pedido {order.OrderId} processado com sucesso");
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"❌ Erro no consumer: {ex.Message}");

                Console.WriteLine(ex.StackTrace);
            }
        }

        consumer.Close();
    }
}