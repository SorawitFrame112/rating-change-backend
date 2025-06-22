using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rtc.Application.Common.InterfacesServices
{
    public interface IRefreshTokenGenerator
    {
        string GenerateRefreshToken();
        DateTime GetRefreshTokenExpiryTime();
    }
}