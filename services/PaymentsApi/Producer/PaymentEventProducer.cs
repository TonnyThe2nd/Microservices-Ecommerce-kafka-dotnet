using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

public class PaymentEventProducer
{
    private readonly IProducer<string, string> _producer;
    private readonly IConfiguration _configuration;

    public PaymentEventProducer(IConfiguration configuration)
    {
        _configuration = configuration;
        var config = new ProducerConfig
        {
            BootstrapServers = _configuration["Kafka:BootstrapServers"]
        };
        _producer = new ProducerBuilder<string, string>(config).Build();
    }   

    public async Task PublishPaymentProcessedAsync(PaymentProcessedEvent paymentEvent)
    {
        var jsonMessage = JsonSerializer.Serialize(paymentEvent);
        var kafkaMessage = new Message<string, string>
        {
            Key = paymentEvent.OrderId.ToString(),
            Value = jsonMessage
        };

        try
        {
            await _producer.ProduceAsync("payment-processed", kafkaMessage);
        }
        catch (ProduceException<string, string> ex)
        {
            Console.WriteLine($"Erro ao enviar evento: {ex.Error.Reason}");
        }
    }
}