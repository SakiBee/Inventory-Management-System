using System.ComponentModel.DataAnnotations;

namespace IMS.DTOs.User
{
    /// <summary>
    /// DTO used to create a user
    /// </summary>
    public class UserCreateDto
    {
        /// <example>Gilman</example>
        [Required(ErrorMessage = "User Name is required")]
        public string Name { get; set; } = string.Empty;

        /// <example>name@ims.com</example>
        [Required(ErrorMessage = "Email Name is required")]
        public string Email { get; set; }

        /// <example>123456</example>
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
