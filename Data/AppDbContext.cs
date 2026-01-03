using Microsoft.EntityFrameworkCore;
using IMS.Models;

namespace IMS.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Laptop", Category = "Electronics", Price = 1200.00m, Quantity = 10, CreatedAt = DateTime.SpecifyKind(new DateTime(2025, 1, 1), DateTimeKind.Utc) },
                new Product { Id = 2, Name = "Mouse", Category = "Electronics", Price = 25.50m, Quantity = 50, CreatedAt = DateTime.SpecifyKind(new DateTime(2025, 1, 1), DateTimeKind.Utc) },
                new Product { Id = 3, Name = "Office Chair", Category = "Furniture", Price = 150.00m, Quantity = 15, CreatedAt = DateTime.SpecifyKind(new DateTime(2025, 1, 1), DateTimeKind.Utc) },
                new Product { Id = 4, Name = "Coffee Mug", Category = "Kitchen", Price = 12.99m, Quantity = 100, CreatedAt = DateTime.SpecifyKind(new DateTime(2025, 1, 1), DateTimeKind.Utc) },
                new Product { Id = 5, Name = "Notebook", Category = "Stationery", Price = 5.00m, Quantity = 200, CreatedAt = DateTime.SpecifyKind(new DateTime(2025, 1, 1), DateTimeKind.Utc) }
            );
        }
    }
}