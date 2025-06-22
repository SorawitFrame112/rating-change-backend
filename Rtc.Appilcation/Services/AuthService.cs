
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Rtc.Appilcation.InterfacesServices;
using Rtc.Application.Common.InterfacesServices;
using Rtc.Application.DTOs.Auth;
using Rtc.Domain.Dtos;
using Rtc.Domain.Dtos.Request;
using Rtc.Domain.Dtos.Response;
using Rtc.Domain.Entities;
using Rtc.Domain.InterfacesServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Rtc.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly RoleManager<ApplicationRole> _roleManager; 
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly IConfiguration _configuration;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtTokenGenerator jwtTokenGenerator,
            RoleManager<ApplicationRole> roleManager,
            IRefreshTokenGenerator refreshTokenGenerator
            ) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _roleManager = roleManager;
            _refreshTokenGenerator = refreshTokenGenerator;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
            {
                throw new ApplicationException("Invalid credentials.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
            {
                throw new ApplicationException("Invalid credentials.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = _jwtTokenGenerator.GenerateToken(user, roles); 

            var refreshToken = _refreshTokenGenerator.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = _refreshTokenGenerator.GetRefreshTokenExpiryTime();
            await _userManager.UpdateAsync(user); 

            return new LoginResponseDto
            {
                UserId = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Token = accessToken, 
                RefreshToken = refreshToken, 
                Roles = roles.ToList()
            };
        }

        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            var user = new ApplicationUser
            {
                UserName = request.Username,
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync("User"))
                {
                    await _roleManager.CreateAsync(new ApplicationRole { Name = "User" });
                }
                await _userManager.AddToRoleAsync(user, "User");

                return new RegisterResponseDto { UserId = user.Id, Username = user.UserName, Email = user.Email, Succeeded = true };
            }
            else
            {
                return new RegisterResponseDto
                {
                    Succeeded = false,
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }
        }

        public async Task<bool> AssignRoleToUserAsync(AssignRoletoUserRequestDto assignRole)
        {
            var user = await _userManager.FindByIdAsync(assignRole.UserId);
            if (user == null) return false;

            if (!await _roleManager.RoleExistsAsync(assignRole.RoleName))
            {
                await _roleManager.CreateAsync(new ApplicationRole { Name = assignRole.RoleName });
            }
            var roleOld = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roleOld);
            
            var result = await _userManager.AddToRoleAsync(user, assignRole.RoleName);
            return result.Succeeded;
        }

        public async Task<LoginResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request)
        {
            var principal = GetPrincipalFromExpiredToken(request.AccessToken);
            var userId = principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                throw new ApplicationException("Invalid access token.");
            }

            var user = await _userManager.FindByIdAsync(userId);

            // ตรวจสอบ Refresh Token
            if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                // ถ้า user ไม่พบ (refreshToken ผูกไม่เจอ user)
                // หรือ RefreshToken ที่ส่งมาไม่ตรงกับที่เก็บไว้ใน DB (การผูก token ไม่ตรง)
                // หรือ RefreshTokenExpiryTime ใน DB หมดอายุแล้ว
                throw new ApplicationException("Invalid or expired refresh token.");

            }

            var roles = await _userManager.GetRolesAsync(user);
            var newAccessToken = _jwtTokenGenerator.GenerateToken(user, roles);

            var newRefreshToken = _refreshTokenGenerator.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = _refreshTokenGenerator.GetRefreshTokenExpiryTime();
            await _userManager.UpdateAsync(user);

            return new LoginResponseDto
            {
                UserId = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Token = newAccessToken,
                RefreshToken = newRefreshToken,
                Roles = roles.ToList()
            };
        }


        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false, // ไม่ต้อง Validate Lifetime สำหรับ Expired Token
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["JwtSettings:Issuer"],
                ValidAudience = _configuration["JwtSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token algorithm.");
            }

            return principal;
        }
    }
}
