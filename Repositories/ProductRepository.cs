using IMS.Data;
using IMS.Models;
using IMS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context) => _context = context;

        public async Task<Product> AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }
        public async Task AddRangeAsync(IEnumerable<Product> products)
        {
            await _context.Products.AddRangeAsync(products);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Product product)
        {
            _context.Products.Remove(product);
            return await _context.SaveChangesAsync() > 0;

        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.AsNoTracking().ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public Task<List<Product>> GetByPriceRangeAsync(decimal? minPrice, decimal? maxPrice)
        {
            var query = _context.Products.AsQueryable();

            if(minPrice.HasValue)
            {
                query = query.Where(p =>  p.Price >= minPrice);
            }
            if(maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice);
            }

            return query.AsNoTracking().ToListAsync();
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> CountAsync()
        {
            return await _context.Products.CountAsync();
        }

        public async Task<List<Product>> GetPagedAsync(int pageNumber, int pageSize)
        {
            return await _context.Products
                .OrderByDescending(p => p.Id)
                .Skip((pageNumber-1) * pageSize)
                .Take(pageSize).ToListAsync();
        }
    }
}
