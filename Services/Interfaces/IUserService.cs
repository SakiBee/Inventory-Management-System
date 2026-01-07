using IMS.DTOs.User;

namespace IMS.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserReadDto>> GetAllAsync();
        Task<UserReadDto?> GetByIdAsync(int id);
        Task AssignRoleAsync(int userId, string role);
    }
}
