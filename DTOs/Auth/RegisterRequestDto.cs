using System.ComponentModel.DataAnnotations;

namespace IMS.DTOs.Auth
{
    /// <summary>
    /// DTO used to register
    /// </summary>
    public class RegisterRequestDto
    {
        /// <example>Jahid</example>
        [Required]
        [MinLength(3)]
        public string Username { get; set; } = null!;

        /// <example>name@ims.com</example>
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <example>123456</example>
        [Required]
        [MinLength (6)]
        public string Password { get; set; }
    }
}
