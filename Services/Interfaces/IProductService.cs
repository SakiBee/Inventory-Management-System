using IMS.DTOs.Product;
using Microsoft.AspNetCore.Mvc;

namespace IMS.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductReadDto> CreateAsync(ProductCreateDTO dto);
        Task<ProductReadDto?> GetByIdAsync(int id);
        Task<IEnumerable<ProductReadDto>> GetAllAsync();
        Task<bool> UpdateAsync(int id, ProductUpdateDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<List<ProductReadDto>> GetProductByPriceRange(decimal? minPrice, decimal? maxPrice);
    }
}