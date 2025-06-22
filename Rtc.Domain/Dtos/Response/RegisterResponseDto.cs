namespace Rtc.Domain.Dtos.Response
{
    public class RegisterResponseDto
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool Succeeded { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }

}
