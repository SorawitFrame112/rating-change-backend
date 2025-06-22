using System.ComponentModel.DataAnnotations;

namespace Rtc.Domain.Dtos.Request
{
    public class LoginRequestDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
