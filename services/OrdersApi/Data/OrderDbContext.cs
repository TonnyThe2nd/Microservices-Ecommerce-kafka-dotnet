using Microsoft.EntityFrameworkCore;

public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
    {
    }

    public DbSet<Orders> Orders { get; set; }
    public DbSet<StockItem> StockItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Orders>(_ =>
        {
            _.ToTable("Orders","OEBM\\antonioso");
            _.HasKey(o => o.Id);
        });
        
        modelBuilder.Entity<OrderItem>(_ =>
        {
            _.ToTable("OrderItems","OEBM\\antonioso");
            _.HasKey(oi => oi.Id);
            _.Property(oi => oi.UnitPrice).HasColumnType("decimal(18,2)");
             _.HasOne<Orders>()
            .WithMany(o => o.Items)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<StockItem>(_ =>
        {
            _.ToTable("StockItems","OEBM\\antonioso");
            _.HasKey(si => si.Id);
            _.Property(si => si.ProductName).IsRequired().HasMaxLength(100);
            _.Property(si => si.Quantity).IsRequired();
        });
    }

}