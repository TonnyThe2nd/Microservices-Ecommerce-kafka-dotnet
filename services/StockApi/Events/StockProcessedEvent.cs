using System;

public class StockProcessedEvent
{
    public int OrderId { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime ProcessedAt { get; set; }
    public List<StockItemInfo> Items { get; set; }
}
public class StockItemInfo
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal UnitPrice { get; set; }
}