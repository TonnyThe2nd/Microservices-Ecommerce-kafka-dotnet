using System;

public class StockProcessedEvent
{
    public int OrderId { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime ProcessedAt { get; set; }
}
