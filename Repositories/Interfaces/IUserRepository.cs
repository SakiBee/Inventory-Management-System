using IMS.Models;

namespace IMS.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> AddAsync(User user);
        Task<bool> ExistsAsync(string username, string email);
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByUsernameOrEmailAsync(string value);
        Task<List<User>> GetAllAsync();
        Task<bool> UpdateAsync(User user);
        Task<bool> DeleteAsync(User user);
    }
}
