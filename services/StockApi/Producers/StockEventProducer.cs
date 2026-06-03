using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

public class StockEventProducer : IStockEventProducer
{
    private readonly IProducer<string, string> _producer;
    private readonly IConfiguration _configuration;

    public StockEventProducer(IConfiguration configuration)
    {
        _configuration = configuration;
        var config = new ProducerConfig
        {
            BootstrapServers = _configuration["Kafka:BootstrapServers"]
        };
        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async Task PublishAsync(StockProcessedEvent evt)
    {
        var message = new Message<string, string>
        {
            Key = evt.OrderId.ToString(),
            Value = JsonSerializer.Serialize(evt)
        };
        await _producer.ProduceAsync("stock-processed", message);
    }
}