public class Orders
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    public decimal Total => Items.Sum(i => i.TotalPrice);
}

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }

    public int ProductId { get; set; }
    public string ProductName { get; set; } 
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public decimal TotalPrice => Quantity * UnitPrice;
}
public enum OrderStatus
{
    Pending,
    Paid,
    Failed,
    StockFailed
}