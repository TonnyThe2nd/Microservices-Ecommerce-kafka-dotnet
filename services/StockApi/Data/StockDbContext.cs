using Microsoft.EntityFrameworkCore;


public class StockDbContext : DbContext
{
    public StockDbContext(DbContextOptions<StockDbContext> options) : base(options)
    {
    }
    public DbSet<Orders> Orders { get; set; }
    public DbSet<StockItem> StockItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StockItem>(_ =>
        {
            _.ToTable("StockItems","OEBM\\antonioso");
            _.HasKey(s => s.Id);
            _.Property(s => s.ProductId).IsRequired().HasMaxLength(100);
        });

            modelBuilder.Entity<Orders>(_ =>
            {
                _.ToTable("Orders","OEBM\\antonioso");
                _.HasKey(o => o.Id);
            });
    }
}