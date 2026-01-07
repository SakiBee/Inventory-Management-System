using IMS.Models;

namespace IMS.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role?> GetByNameAsync(string name);
        Task<bool> AddAsync(Role role);
    }
}
