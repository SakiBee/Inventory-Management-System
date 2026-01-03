using IMS.Data;
using IMS.Models;
using System.Linq;
using IMS.DTOs.Product;
using IMS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace IMS.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        public ProductService(AppDbContext context) => _context = context;

        public async Task<ProductReadDto> CreateAsync(ProductCreateDTO dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Category = dto.Category,
                Price = dto.Price,
                Quantity = dto.Quantity
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return MapToReadDto(product);
        }

        public async Task<ProductReadDto?> GetByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            return product == null ? null : MapToReadDto(product);
        }

        public async Task<IEnumerable<ProductReadDto>> GetAllAsync()
        {
            var products = await _context.Products.ToListAsync();
            return products.Select(MapToReadDto);
        }

        public async Task<bool> UpdateAsync(int id, ProductUpdateDTO dto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            product.Name = dto.Name;
            product.Category = dto.Category;
            product.Price = dto.Price;
            product.Quantity = dto.Quantity;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        private static ProductReadDto MapToReadDto(Product product) => new()
        {
            Id = product.Id,
            Name = product.Name,
            Category = product.Category,
            Price = product.Price,
            Quantity = product.Quantity
        };

        async Task<List<ProductReadDto>> IProductService.GetProductByPriceRange(decimal? minPrice, decimal? maxPrice)
        {
            var query = _context.Products.AsQueryable();
            if(minPrice.HasValue) query = query.Where(x => x.Price >= minPrice.Value);
            if(maxPrice.HasValue) query = query.Where(x => x.Price <= maxPrice.Value);

            return await query.Select(p => new ProductReadDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Category = p.Category,
                    Price = p.Price,
                    Quantity = p.Quantity
                }).ToListAsync();
        }

    }
}