using Microsoft.AspNetCore.Identity;

namespace PerformanceManagementSystem.Data.Models;

public class AppRole : IdentityRole<Guid>
{
    public ICollection<AppUserRole> UserRoles { get; set; }
}