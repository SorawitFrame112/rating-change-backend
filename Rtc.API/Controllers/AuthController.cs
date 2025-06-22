using Microsoft.AspNetCore.Mvc;
using Rtc.Domain.Dtos;
using Rtc.Domain.Dtos.Request;
using Rtc.Domain.InterfacesServices;


namespace Rtc.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterRequest([FromBody] RegisterRequestDto request )
        {
            var response = await _authService.RegisterAsync(request);
            return Ok(response);
        }

        [HttpPost("AssignRoletoUser")]
        public async Task<IActionResult> AssignRoletoUser([FromBody] AssignRoletoUserRequestDto request)
        {
            var response = await _authService.AssignRoleToUserAsync(request);
            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var response = await _authService.LoginAsync(request);
            return Ok(response);
        }

    }
}