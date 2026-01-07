using System.ComponentModel.DataAnnotations;

namespace IMS.DTOs.Auth
{
    public class RegisterRequestDto
    {
        [Required]
        [MinLength(3)]
        public string Username { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength (6)]
        public string Password { get; set; }
    }
}
