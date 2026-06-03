public record OrderCreatedEvent(
    int OrderId,
    string CustomerName,
    string CustomerEmail,
    List<OrderCreatedItem> Items,
    decimal Total,
    DateTime CreatedAt
);
 
public record OrderCreatedItem(
    int ProductId,
    int Quantity,
    decimal UnitPrice
);