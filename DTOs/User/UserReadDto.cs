namespace IMS.DTOs.User
{
    /// <summary>
    /// DTO used to Read a user
    /// </summary>
    public class UserReadDto
    {
        /// <example>1</example>
        public int Id { get; set; }
        /// <example>Nibir</example>
        public string Username { get; set; } = string.Empty;
        /// <example>name@ims.com</example>
        public string Email { get; set; } = string.Empty;
        /// <example>1</example>
        public bool IsActive { get; set; }
        /// <example>User</example>
        public List<string> Roles { get; set; } = new();
    }
}
