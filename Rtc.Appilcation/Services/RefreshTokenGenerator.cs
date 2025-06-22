
using Rtc.Application.Common.InterfacesServices;
using System;
using System.Security.Cryptography; // For RandomNumberGenerator

namespace Rtc.Application.Services
{
    public class RefreshTokenGenerator : IRefreshTokenGenerator
    {
        private readonly int _refreshTokenValidityInDays;

        public RefreshTokenGenerator(int refreshTokenValidityInDays = 2)
        {
            _refreshTokenValidityInDays = refreshTokenValidityInDays;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public DateTime GetRefreshTokenExpiryTime()
        {
            return DateTime.UtcNow.AddDays(_refreshTokenValidityInDays);
        }
    }
}