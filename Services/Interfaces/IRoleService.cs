using IMS.Models;

namespace IMS.Services.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<Role>> GetAllAsync();
        Task CreateAsync(string roleName);
    }
}
