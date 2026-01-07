using IMS.Data;
using IMS.Models;
using System.Linq;
using IMS.DTOs.Product;
using IMS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using IMS.Repositories.Interfaces;
using CsvHelper;
using System.Globalization;
using IMS.DTOs.Common;

namespace IMS.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<ProductService> _logger;
        public ProductService(IProductRepository repository, ILogger<ProductService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ProductReadDto> CreateAsync(ProductCreateDTO dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Category = dto.Category,
                Price = dto.Price,
                Quantity = dto.Quantity,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(product);

            _logger.LogInformation(
                "Product created | Id: {Id} | Name: {Name} | Category: {Category} | Time: {Time}",

                product.Id,
                product.Name,
                product.Category,
                product.CreatedAt
             );

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

        public async Task<int> UploadProductFromCsvAsync(Stream fileStream)
        {
            using var reader = new StreamReader(fileStream);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<ProductCsvDto>().ToList();

            var products = records.Select(r => new Product
            {
                Name = r.Name,
                Category = r.Category,
                Price = r.Price,
                Quantity = r.Quantity,
                CreatedAt = DateTime.Now,
            }).ToList();

            if(records.Any())
            {
                await _repository.AddRangeAsync(products);
            }
            return records.Count;
        }

        public async Task<PagedResultDto<ProductReadDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            pageNumber = pageNumber <= 0 ? 0 : pageNumber;
            pageSize = pageSize <= 0 ? 10 : pageSize;

            var totalCount = await _repository.CountAsync();
            var products = await _repository.GetPagedAsync(pageNumber, pageSize);

            return new PagedResultDto<ProductReadDto>
            {
                Items = products.Select(MapToReadDto),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}