using Rtc.Domain.Entities;


namespace Rtc.Appilcation.InterfacesServices
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken( ApplicationUser user, IList<string> roles);
    }
}
