public interface IStockEventProducer
{
    Task PublishAsync(StockProcessedEvent message);
}