public record CreateOrderRequest(
    string CustomerName,
    string CustomerEmail,
    List<CreateOrderItemRequest> Items
);

public record CreateOrderItemRequest(
    int ProductId,
    int Quantity
);

public record OrderResponse(
    int Id,
    string CustomerName,
    string CustomerEmail,
    OrderStatus Status,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    List<OrderItemResponse> Items,
    decimal Total
);

public record OrderItemResponse(
    int ProductId,
    int Quantity,
    decimal UnitPrice
);