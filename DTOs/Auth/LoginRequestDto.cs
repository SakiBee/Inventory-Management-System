using System.ComponentModel.DataAnnotations;

namespace IMS.DTOs.Auth
{
    /// <summary>
    /// DTO used to Login
    /// </summary>
    public class LoginRequestDto
    {
        /// <example>Gilman</example>
        [Required]
        public string UsernameOrEmail { get; set; } = null!;

        /// <example>123456</example>
        [Required]
        public string Password { get; set; } = null!;
    }
}
