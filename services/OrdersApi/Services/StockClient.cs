using Microsoft.EntityFrameworkCore;

public class StockClient
{
    private readonly OrderDbContext _context;

    public StockClient(OrderDbContext context)
    {
        _context = context;
    }

    public async Task<StockItem?> GetProductAsync(int productId)
    {
        return await _context.StockItems
            .FirstOrDefaultAsync(x => x.ProductId == productId);
    }
}