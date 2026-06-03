using Confluent.Kafka;
using System.Text.Json;

public class StockProcessedConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _configuration;

    public StockProcessedConsumer(IServiceScopeFactory scopeFactory, IConfiguration configuration)
    {
        _scopeFactory = scopeFactory;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _configuration["Kafka:BootstrapServers"],
            GroupId = "payments-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<string, string>(config).Build();
        consumer.Subscribe("stock-processed");

        while (!stoppingToken.IsCancellationRequested)
        {
            var result = consumer.Consume(stoppingToken);
            Console.WriteLine("📥 [PaymentsApi] Recebeu stock-processed");

            var stockEvent = JsonSerializer.Deserialize<StockProcessedEvent>(result.Message.Value);
            Console.WriteLine($"📦 OrderId: {stockEvent.OrderId} | Success: {stockEvent.Status} | Amount: {stockEvent.Amount} ");

            if (stockEvent == null || stockEvent.Status == StockStatus.Failed){
                Console.WriteLine("⚠️ [PaymentsApi] Estoque falhou, pagamento cancelado");
                continue;
            }
            Console.WriteLine("💳 [PaymentsApi] Processando pagamento...");

            using var scope = _scopeFactory.CreateScope();

            var paymentService = scope.ServiceProvider.GetRequiredService<PaymentService>();
            var producer = scope.ServiceProvider.GetRequiredService<PaymentEventProducer>();

            var paymentStatus = paymentService.ProcessPayment(stockEvent.OrderId, stockEvent.Amount);
            Console.WriteLine(paymentStatus == PaymentStatus.Success
                ? "✅ [PaymentsApi] Pagamento aprovado"
                : "❌ [PaymentsApi] Pagamento recusado");

            var evt = new PaymentProcessedEvent
            {
                OrderId = stockEvent.OrderId,
                Amount = stockEvent.Amount,
                Status = paymentStatus,
                ProcessedAt = DateTime.UtcNow
            };

            await producer.PublishPaymentProcessedAsync(evt);
            Console.WriteLine("📤 [PaymentsApi] Publicando payment-processed");
        }

        consumer.Close();
    }
}