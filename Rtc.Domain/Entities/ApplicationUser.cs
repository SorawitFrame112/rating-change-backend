


using Microsoft.AspNetCore.Identity; 

namespace Rtc.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() : base()
        {
        }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }  
        public DateTime? DateOfBirth { get; set; } 
        public string? ProfilePictureUrl { get; set; } 
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow; 
        public DateTime? LastLoginDate { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }

    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base()
        {
        }
    }
}