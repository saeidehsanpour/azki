namespace PerformanceManagementSystem.Data.Models;

public class Organization : Entity
{
    public Guid AdminUserId { get; set; }
    public virtual AppUser AdminUser { get; set; } = null!;
}