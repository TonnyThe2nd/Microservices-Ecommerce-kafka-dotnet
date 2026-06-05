public class Orders
{
    public int Id { get; set; }
    public Guid CustomerId { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    public decimal Total {get; set;}
}

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }

    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public decimal TotalPrice {get; set;}
}

public enum OrderStatus
{
    VerifyingStock,
    Pending,
    Paid,
    Failed,
    StockFailed
}