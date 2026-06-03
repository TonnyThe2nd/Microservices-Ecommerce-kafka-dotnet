public class PaymentProcessedEvent
{
    public int OrderId { get; set; }
    public PaymentStatus Status { get; set; }
    public decimal Amount { get; set; }
    public DateTime ProcessedAt { get; set; }
}


public enum PaymentStatus
{
    Success,
    Failed,
    Rejected,
    Pending
}