using IMS.DTOs.Auth;
using IMS.Models;
using IMS.Repositories.Interfaces;
using IMS.Services.Interfaces;

namespace IMS.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthService(IUserRepository userRepository, IRoleRepository roleRepository, IPasswordHasher passwordHasher, IJwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto)
        {
            if (await _userRepository.ExistsAsync(dto.Username, dto.Email))
                throw new Exception("User already exists");

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = _passwordHasher.Hash(dto.Password)
            };

            await _userRepository.AddAsync(user);

            var defaultRole = await _roleRepository.GetByNameAsync("User");
            if (defaultRole != null)
            {
                var userRole = new UserRole
                {
                    UserId = user.Id,
                    RoleId = defaultRole.Id,
                    Role = defaultRole
                };
                user.UserRoles.Add(userRole);
                await _userRepository.UpdateAsync(user);
            }

            var token = _jwtTokenService.GenerateToken(user);

            return new AuthResponseDto
            {
                UserId = user.Id,
                UserName = user.Username,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(2)
            };
        }
        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto)
        {
            var user = await _userRepository.GetByUsernameOrEmailAsync(dto.UsernameOrEmail);
            if(user ==  null)
            {
                throw new Exception("Invalid credentials");
            }

            if (!_passwordHasher.Verify(dto.Password, user.PasswordHash))
            {
                throw new Exception("Invalid credentials");
            }

            var token = _jwtTokenService.GenerateToken(user);

            return new AuthResponseDto
            {
                UserId = user.Id,
                UserName = user.Username,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(2)
            };
                
        }
    }
}
