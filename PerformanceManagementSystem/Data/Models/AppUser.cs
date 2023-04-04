using Microsoft.AspNetCore.Identity;

namespace PerformanceManagementSystem.Data.Models;

public class AppUser : IdentityUser<Guid>
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public bool Active { get; set; }
    public Guid? PositionId { get; set; }
    public virtual Position Position { get; set; }
    public string FullName => $"{Firstname} {Lastname}";
    public DateTimeOffset? RegisterDate { get; set; }
    public virtual ICollection<AppUserRole> UserRoles { get; set; }
    public virtual ICollection<AppUserClaim> UserClaims { get; set; }
    public virtual ICollection<PerformanceManagementPeriodUserMapping> PerformanceManagementPeriodUserMappings { get; set; }
    public virtual ICollection<TaskUserMention> TaskUserMentions { get; set; }
    public virtual ICollection<UserManagerMapping> Users { get; set; }
    public virtual ICollection<UserManagerMapping> Managers { get; set; }
    public AppUser()
    {
        RegisterDate = DateTimeOffset.UtcNow;
        PerformanceManagementPeriodUserMappings = new HashSet<PerformanceManagementPeriodUserMapping>();
        UserClaims = new HashSet<AppUserClaim>();
        UserRoles = new HashSet<AppUserRole>();
        TaskUserMentions = new HashSet<TaskUserMention>();
    }
}