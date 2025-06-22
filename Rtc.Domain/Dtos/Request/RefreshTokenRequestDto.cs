using System.ComponentModel.DataAnnotations;

namespace Rtc.Application.DTOs.Auth
{
    public class RefreshTokenRequestDto
    {
        [Required]
        public string AccessToken { get; set; } // JWT ที่หมดอายุ (หรือใกล้หมดอายุ)
        [Required]
        public string RefreshToken { get; set; }
    }
}