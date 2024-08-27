using Microsoft.EntityFrameworkCore;

namespace StockService.Persistence.Stock
{
    public class StockDbContext : DbContext
    {
        public StockDbContext(DbContextOptions<StockDbContext> options) : base(options) { }

        public DbSet<Domain.Stock.Entities.Stock> Stock{ get; set; }
        public DbSet<Domain.Stock.Entities.Product> Product{ get; set; }
        public DbSet<Domain.Stock.Entities.Cart> Cart{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.Stock.Entities.Stock>().HasKey(o => o.stockId);
            modelBuilder.Entity<Domain.Stock.Entities.Product>().HasKey(o => o.prodId);
            modelBuilder.Entity<Domain.Stock.Entities.Cart>().HasKey(o => o.cartId);
        }
    }
}
