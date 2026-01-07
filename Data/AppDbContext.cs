using Microsoft.EntityFrameworkCore;
using IMS.Models;

namespace IMS.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<User> User => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
             
            modelBuilder.Entity<UserRole>().HasKey(ur => new { ur.UserId, ur.RoleId });
            modelBuilder.Entity<UserRole>().HasOne(u => u.User).WithMany(u => u.UserRoles).HasForeignKey(u => u.UserId);
            modelBuilder.Entity<UserRole>().HasOne(r => r.Role).WithMany(r => r.UserRoles).HasForeignKey(r => r.RoleId);

            //Unique Constrains
            modelBuilder.Entity<User>().HasIndex(r => r.Username).IsUnique();
            modelBuilder.Entity<User>().HasIndex(r => r.Email).IsUnique();
            modelBuilder.Entity<Role>().HasIndex(r => r.Name).IsUnique();
        }
    }
}