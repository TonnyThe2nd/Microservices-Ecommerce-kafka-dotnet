using Confluent.Kafka;
using System.Text.Json;

public class KafkaProducer : IDisposable
{
    private readonly IProducer<string,string> _producer;
    private readonly ILogger<KafkaProducer> _logger;
    public KafkaProducer(IConfiguration configuration, ILogger<KafkaProducer> logger)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = configuration["Kafka:BootstrapServers"]
        };
        _producer = new ProducerBuilder<string, string>(config).Build();
        _logger = logger;
    }

    public async Task PublishAsync<T>(string topic, string key, T message)
    {
        var jsonMessage = JsonSerializer.Serialize(message);
        var kafkaMessage = new Message<string,string> {Key= key, Value = jsonMessage};

        var result = await _producer.ProduceAsync(topic, kafkaMessage);
        _logger.LogInformation("Evento publicado no tópico {Topic} | Offset: {Offset}", topic, result.Offset);

    }

    public void Dispose() => _producer.Dispose();
}