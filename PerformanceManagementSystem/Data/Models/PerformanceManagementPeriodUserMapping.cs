namespace PerformanceManagementSystem.Data.Models;

public class PerformanceManagementPeriodUserMapping : Entity
{
    public PerformanceManagementPeriodUserMapping()
    {
        TaskOfPeriods = new HashSet<TaskOfPeriod>();
    }
    public Guid PerformanceManagementPeriodId { get; set; }
    public Guid UserId { get; set; }
    public virtual PerformanceManagementPeriod PerformanceManagementPeriod { get; set; } = null!;
    public AppUser User { get; set; } = null!;
    public virtual ICollection<TaskOfPeriod> TaskOfPeriods { get; set; }
}