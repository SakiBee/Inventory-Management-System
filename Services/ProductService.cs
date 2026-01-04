using IMS.Data;
using IMS.Models;
using System.Linq;
using IMS.DTOs.Product;
using IMS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using IMS.Repositories.Interfaces;

namespace IMS.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        public ProductService(IProductRepository repository) => _repository = repository;

        public async Task<ProductReadDto> CreateAsync(ProductCreateDTO dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Category = dto.Category,
                Price = dto.Price,
                Quantity = dto.Quantity
            };

            await _repository.AddAsync(product);
            return MapToReadDto(product);
        }

        public async Task<ProductReadDto?> GetByIdAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            return product == null ? null : MapToReadDto(product);
        }

        public async Task<IEnumerable<ProductReadDto>> GetAllAsync()
        {
            var products = await _repository.GetAllAsync();
            return products.Select(MapToReadDto);
        }

        public async Task<bool> UpdateAsync(int id, ProductUpdateDTO dto)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null) return false;

            product.Name = dto.Name;
            product.Category = dto.Category;
            product.Price = dto.Price;
            product.Quantity = dto.Quantity;

            return await _repository.UpdateAsync(product);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null) return false;

            return await _repository.DeleteAsync(product);
        }

        private static ProductReadDto MapToReadDto(Product product) => new()
        {
            Id = product.Id,
            Name = product.Name,
            Category = product.Category,
            Price = product.Price,
            Quantity = product.Quantity
        };

        public async Task<List<ProductReadDto>> GetProductByPriceRange(decimal? minPrice, decimal? maxPrice)
        {
            var products = await _repository.GetByPriceRangeAsync(minPrice, maxPrice);
            return products.Select(MapToReadDto).ToList();
        }

    }
}