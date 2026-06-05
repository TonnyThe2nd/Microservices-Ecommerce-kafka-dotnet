public record OrderCreatedEvent(
    int OrderId,
    Guid CustomerId,
    List<OrderCreatedItem> Items,
    decimal Total,
    DateTime CreatedAt
);
 
public record OrderCreatedItem(
    int ProductId,
    int Quantity,
    decimal UnitPrice
);