namespace IMS.DTOs.Auth
{
    public class AuthResponseDto
    {
        public int UserId {  get; set; }
        public string UserName { get; set; } = null!;
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }

    }
}
