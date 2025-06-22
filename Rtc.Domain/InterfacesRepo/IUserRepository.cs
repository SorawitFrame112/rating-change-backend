using Rtc.Domain.Entities;

namespace Rtc.Domain.InterfacesRepo
{
    public interface IUserRepository
    {

      Task<ApplicationUser> GetUserByUsernameAsync(string username);

      Task<ApplicationUser?> GetUserByIdAsync(string userId);

       Task AddUserAsync(ApplicationUser user, string password);

    }
}