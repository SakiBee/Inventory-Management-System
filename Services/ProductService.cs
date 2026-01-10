using AutoMapper;
using CsvHelper;
using IMS.Data;
using IMS.DTOs.Common;
using IMS.DTOs.Product;
using IMS.Models;
using IMS.Repositories.Interfaces;
using IMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Globalization;
using System.Linq;

namespace IMS.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<ProductService> _logger;
        private readonly IMapper _mapper;
        public ProductService(IProductRepository repository, ILogger<ProductService> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ProductReadDto> CreateAsync(ProductCreateDTO dto)
        {
            //var product = new Product
            //{
            //    Name = dto.Name,
            //    Category = dto.Category,
            //    Price = dto.Price,
            //    Quantity = dto.Quantity,
            //    CreatedAt = DateTime.UtcNow
            //};
            var product = _mapper.Map<Product>(dto);
            if (product.Price < 0) throw new ArgumentException("Price cannot be negative");
            product.CreatedAt = DateTime.Now;

            await _repository.AddAsync(product);

            _logger.LogInformation(
                "Product created | Id: {Id} | Name: {Name} | Category: {Category} | Time: {Time}",
                product.Id,
                product.Name,
                product.Category,
                product.CreatedAt
             );

            //return MapToReadDto(product);
            return _mapper.Map<ProductReadDto>(product);
        }

        public async Task<ProductReadDto?> GetByIdAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            return product == null ? null : _mapper.Map<ProductReadDto>(product);
        }

        public async Task<IEnumerable<ProductReadDto>> GetAllAsync()
        {
            var products = await _repository.GetAllAsync();
            return products.Select(MapToReadDto);
            //return _mapper.Map<IEnumerable<ProductReadDto>>(products);
        }

        public async Task<bool> UpdateAsync(int id, ProductUpdateDTO dto)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null) return false;

            _mapper.Map(dto, product);

            bool isUpdated = await _repository.UpdateAsync(product);

            return isUpdated;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null) return false;

            return await _repository.DeleteAsync(product);
        }

        //Manual Mapper
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
            //return products.Select(MapToReadDto).ToList();

            var dtoList = _mapper.Map<List<ProductReadDto>>(products);
            return dtoList;
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

            var dtoItems = _mapper.Map<List<ProductReadDto>>(products);

            return new PagedResultDto<ProductReadDto>
            {
                //Items = products.Select(MapToReadDto), //Manual Mapping
                Items = dtoItems,  // Auto Mapping
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        /*public async Task<List<Product>> GetPagedAsync(int pageNumber, int pageSize)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 10 : pageSize;

            var offset = (pageNumber - 1) * pageSize;

            return await _context.Products
                .FromSqlRaw(
                    @"SELECT * FROM ""Products""
                      ORDER BY ""Id"" DESC
                      LIMIT @limit OFFSET @offset",
                    new NpgsqlParameter("limit", pageSize),
                    new NpgsqlParameter("offset", offset)
                )
                .AsNoTracking()
                .ToListAsync();
        }
        */
    }
}