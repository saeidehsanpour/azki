namespace PerformanceManagementSystem.Data.Models;

public class TaskUserMention : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid TaskOfPeriodId { get; set; }
    public bool Manager { get; set; }
    public virtual AppUser User { get; set; } = null!;
    public virtual TaskOfPeriod TaskOfPeriod { get; set; } = null!;
}