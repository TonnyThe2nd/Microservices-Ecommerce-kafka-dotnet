using System;
using System.Threading.Tasks;

public class OrderEventProducer
{
    private readonly KafkaProducer _kafkaProducer;

    public OrderEventProducer(KafkaProducer kafkaProducer)
    {
        _kafkaProducer = kafkaProducer;
    }

    public Task PublishPaymentRequestedAsync(int orderId, Guid customerId)
    {
        var evt = new
        {
            OrderId = orderId,
            CustomerId = customerId,
            RequestedAt = DateTime.UtcNow
        };

        return _kafkaProducer.PublishAsync("payment-requested", orderId.ToString(), evt);
    }
}
