namespace PerformanceManagementSystem.Data.Models;

public class UserManagerMapping
{
    public Guid UserId { get; set; }
    public Guid ManagerId { get; set; }
    public virtual AppUser User { get; set; } = null!;
    public virtual AppUser Manager { get; set; } = null!;
}