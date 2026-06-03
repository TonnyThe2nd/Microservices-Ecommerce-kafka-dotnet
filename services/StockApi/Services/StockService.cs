using Microsoft.EntityFrameworkCore;

public class StockService 
{
    private readonly StockDbContext _context;
    private readonly IConfiguration _configuration;

    public StockService(StockDbContext context, IConfiguration configuration)
    {
        _configuration = configuration;
        _context = context;
    }

    public async Task<bool> ReserveStockAsync(int productId,int quantity)
    {
        var item = await _context.StockItems
            .FirstOrDefaultAsync(x => x.ProductId == productId);

        if (item == null)
            return false;

        if (item.Quantity < quantity)
            return false;

        item.Quantity -= quantity;

        return true;
    }
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}