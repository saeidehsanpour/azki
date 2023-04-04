using Microsoft.AspNetCore.Identity;

namespace PerformanceManagementSystem.Data.Models
{
    public class AppUserClaim : IdentityUserClaim<Guid>
    {
        public virtual AppUser AppUser { get; set; }
    }
}
