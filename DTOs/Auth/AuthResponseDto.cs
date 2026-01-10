namespace IMS.DTOs.Auth
{
    /// <summary>
    /// DTO used to get Auth response
    /// </summary>
    public class AuthResponseDto
    {
        /// <example>1</example>
        public int UserId {  get; set; }
        /// <example>Nibir</example>
        public string UserName { get; set; } = null!;
        /// <example>asfsdfggasfgdadfsadf</example>
        public string Token { get; set; }
        /// <example>12.34</example>
        public DateTime ExpiresAt { get; set; }

    }
}
