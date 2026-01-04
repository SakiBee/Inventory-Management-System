using IMS.Models;

namespace IMS.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> AddAsync(Product product);
        Task<Product?> GetByIdAsync(int id);
        Task<List<Product>> GetAllAsync();
        Task<bool> UpdateAsync(Product product);
        Task<bool> DeleteAsync(Product product);
        Task<List<Product>> GetByPriceRangeAsync(decimal? minPrice, decimal? maxPrice);
    }
}
