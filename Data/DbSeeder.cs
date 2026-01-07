using IMS.Models;
using IMS.Services;
using IMS.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IMS.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await context.Database.MigrateAsync();

            if(!await context.Roles.AnyAsync())
            {
                context.Roles.AddRange(
                    new Role { Name = "Admin" },
                    new Role { Name = "Manager" },
                    new Role { Name = "User" }
                    );
                await context.SaveChangesAsync();
            }

            if(!await context.User.AnyAsync())
            {
                var passwordHasher = new PasswordHasher<User>();
                var admin = new User { Username = "admin", Email = "admin@ims.com" };
                admin.PasswordHash = passwordHasher.HashPassword(admin, "admin");

                var user = new User { Username = "user", Email = "user@ims.com" };
                user.PasswordHash = passwordHasher.HashPassword(user, "user");

                context.User.AddRange(admin, user);
                await context.SaveChangesAsync();

                var adminRole = await context.Roles.FirstAsync(r => r.Name == "Admin");
                var useRole = await context.Roles.FirstAsync(r => r.Name == "User");

                context.UserRoles.AddRange(
                    new UserRole { UserId = admin.Id, RoleId = adminRole.Id },
                    new UserRole { UserId = user.Id, RoleId = useRole.Id }
                );
                await context.SaveChangesAsync();
            }

            if (!await context.Products.AnyAsync())
            {
                context.Products.AddRange(
                    new Product { Name = "Laptop", Category = "Electronics", Price = 1200.00m, Quantity = 10, CreatedAt = DateTime.UtcNow },
                    new Product { Name = "Mouse", Category = "Electronics", Price = 25.50m, Quantity = 50, CreatedAt = DateTime.UtcNow },
                    new Product { Name = "Office Chair", Category = "Furniture", Price = 150.00m, Quantity = 15, CreatedAt = DateTime.UtcNow },
                    new Product { Name = "Coffee Mug", Category = "Kitchen", Price = 12.99m, Quantity = 100, CreatedAt = DateTime.UtcNow },
                    new Product { Name = "Notebook", Category = "Stationery", Price = 5.00m, Quantity = 200, CreatedAt = DateTime.UtcNow }
                );
                await context.SaveChangesAsync();
            }
        }
    }
}
