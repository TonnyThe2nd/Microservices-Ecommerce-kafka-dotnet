public class StockProcessedEvent
{
    public int OrderId { get; set; }
    public string ProductId { get; set; }
    public int Quantity { get; set; }

    public StockStatus Status { get; set; }

    public DateTime ProcessedAt { get; set; }
    public decimal Amount { get; set; }
}
public enum StockStatus
{
    Reserved,
    OutOfStock,
    Failed
}