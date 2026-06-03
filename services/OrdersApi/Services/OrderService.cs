
public class OrderService 
{
    private readonly OrderDbContext _context;
    private readonly KafkaProducer _kafkaProducer;
    private readonly StockClient _stockClient;

    public OrderService(OrderDbContext context, KafkaProducer kafkaProducer, StockClient stockClient)
    {
        _context = context;
        _kafkaProducer = kafkaProducer;
        _stockClient = stockClient;
    }
    private decimal CalculateUnitPrice(StockItem product, int quantity)
    {
        decimal price = product.UnitPrice;

        if (quantity >= 10)
            price *= 0.90m; // 10% desconto
        return price;
    }
    public async Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request)
{
    var order = new Orders
    {
        CustomerName = request.CustomerName,
        CustomerEmail = request.CustomerEmail,
        Status = OrderStatus.VerifyingStock,
        CreatedAt = DateTime.UtcNow
    };

    foreach (var item in request.Items)
    {
        order.Items.Add(new OrderItem
        {
            ProductId = item.ProductId,
            Quantity = item.Quantity
        });
    }

    _context.Orders.Add(order);

    await _context.SaveChangesAsync();

    var orderCreatedEvent = new OrderCreatedEvent(
        order.Id,
        order.CustomerName,
        order.CustomerEmail,
        order.Items.Select(i =>
            new OrderCreatedItem(
                i.ProductId,
                i.Quantity, i.UnitPrice
            )).ToList(),
        order.Total,
        order.CreatedAt
    );

    await _kafkaProducer.PublishAsync(
        "order-created",
        order.Id.ToString(),
        orderCreatedEvent
    );

    return new OrderResponse(
        order.Id,
        order.CustomerName,
        order.CustomerEmail,
        order.Status,
        order.CreatedAt,
        order.UpdatedAt,
        order.Items.Select(i =>
            new OrderItemResponse(
                i.ProductId,
                i.Quantity, i.UnitPrice
            )).ToList(),
        order.Total
    );
}
}