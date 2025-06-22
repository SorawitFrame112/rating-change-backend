namespace Rtc.Domain.Dtos.Response
{
    public class LoginResponseDto
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
