using Rtc.Application.DTOs.Auth;
using Rtc.Domain.Dtos;
using Rtc.Domain.Dtos.Request;
using Rtc.Domain.Dtos.Response;

namespace Rtc.Domain.InterfacesServices
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
        Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request);
        Task<bool> AssignRoleToUserAsync(AssignRoletoUserRequestDto assignRole);
        Task<LoginResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request);
    }
}
