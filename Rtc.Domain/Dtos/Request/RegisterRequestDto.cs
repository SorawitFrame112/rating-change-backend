using System.ComponentModel.DataAnnotations;

namespace Rtc.Domain.Dtos.Request
{
    public class RegisterRequestDto
    {
        [Required]
        public string Username { get; set; }
       
        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }
    }

}
