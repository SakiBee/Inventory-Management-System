using IMS.Data;
using IMS.Models;
using IMS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AddAsync(User user)
        {
            _context.User.Add(user);
            return await _context.SaveChangesAsync() > 0;
        }
        public Task<List<User>> GetAllAsync()
        {
            return _context.User.AsNoTracking().ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.User.AsNoTracking().Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByUsernameOrEmailAsync(string value)
        {
            return await _context.User.AsNoTracking().Include(x => x.UserRoles).ThenInclude(x => x.Role)
                .FirstOrDefaultAsync(u => u.Username == value || u.Email == value);
        }

        public async Task<bool> DeleteAsync(User user)
        {
            _context.User.Remove(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public Task<bool> ExistsAsync(string username, string email)
        {
            return _context.User.AnyAsync(u => u.Username == username || u.Email == email);
        }

        public async Task<bool> UpdateAsync(User user)
        {
            _context.User.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
