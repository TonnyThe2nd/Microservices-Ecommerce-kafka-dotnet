using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

public class OrderEventsConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<OrderEventsConsumer> _logger;

    public OrderEventsConsumer(
        IServiceScopeFactory scopeFactory,
        IConfiguration configuration,
        ILogger<OrderEventsConsumer> logger)
    {
        _scopeFactory = scopeFactory;
        _configuration = configuration;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _configuration["Kafka:BootstrapServers"],
            GroupId = "orders-group",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false 
        };

        using var consumer =
            new ConsumerBuilder<string, string>(config)
                .Build();

        consumer.Subscribe("stock-processed");

        _logger.LogInformation("📦 Orders Consumer iniciado");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var result = consumer.Consume(stoppingToken);

                var evt =
                    JsonSerializer.Deserialize<StockProcessedEvent>(
                        result.Message.Value);

                if (evt == null)
                    continue;

                _logger.LogInformation(
                    $"📥 Evento recebido para pedido {evt.OrderId}");

                using var scope =
                    _scopeFactory.CreateScope();

                var context =
                    scope.ServiceProvider
                        .GetRequiredService<OrderDbContext>();

                var producer =
                    scope.ServiceProvider
                        .GetRequiredService<OrderEventProducer>();

                await using var transaction =
                    await context.Database
                        .BeginTransactionAsync(stoppingToken);

                try
                {
                    var order =
                        await context.Orders
                            .Include(o => o.Items)
                            .FirstOrDefaultAsync(
                                o => o.Id == evt.OrderId,
                                stoppingToken);

                    if (order == null)
                    {
                        _logger.LogWarning(
                            $"❌ Pedido {evt.OrderId} não encontrado");
                        continue;
                    }

                    if (evt.Success)
                    {
                        var productIds = order.Items
                            .Select(i => i.ProductId)
                            .Distinct()
                            .ToList();

                        var stockItems =
                            await context.StockItems
                                .AsNoTracking()
                                .Where(s => productIds.Contains(s.ProductId))
                                .ToDictionaryAsync(
                                    s => s.ProductId,
                                    s => s, 
                                    stoppingToken);

                        decimal totalPedido = 0;

                        foreach (var item in order.Items)
                        {
                            if (!stockItems.TryGetValue(item.ProductId, out var stockItem))
                                throw new Exception($"Produto {item.ProductId} não encontrado no estoque");

                            item.UnitPrice = stockItem.UnitPrice;
                            
                            item.TotalPrice = item.UnitPrice * item.Quantity;
                            
                            _logger.LogDebug(
                                $"Item {item.ProductId}: " +
                                $"Qtd: {item.Quantity}, " +
                                $"Preço Unit: {item.UnitPrice:C}, " +
                                $"Total Item: {item.TotalPrice:C}");

                            totalPedido += item.TotalPrice;
                        }

                        order.Total = totalPedido;
                        order.Status = OrderStatus.Pending;
                        order.UpdatedAt = DateTime.UtcNow; 

                        await context.SaveChangesAsync(stoppingToken);

                        await transaction.CommitAsync(stoppingToken);

                        await producer.PublishPaymentRequestedAsync(
                            order.Id,
                            order.CustomerEmail);

                        _logger.LogInformation(
                            $"✅ Pedido {order.Id} atualizado. " +
                            $"Total: {order.Total:C}, " +
                            $"Itens: {order.Items.Count}");
                    }
                    else
                    {
                        order.Status = OrderStatus.StockFailed;
                        order.UpdatedAt = DateTime.UtcNow;

                        await context.SaveChangesAsync(stoppingToken);
                        await transaction.CommitAsync(stoppingToken);

                        _logger.LogWarning(
                            $"❌ Pedido {order.Id} cancelado por falta de estoque");
                    }

                    consumer.Commit(result);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(stoppingToken);
                    _logger.LogError(ex, $"Erro ao processar pedido {evt.OrderId}");
                    throw;
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Consumer cancelado");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Erro ao processar mensagem");
            }
        }

        consumer.Close();
    }
}