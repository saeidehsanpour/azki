namespace PerformanceManagementSystem.Data.Models;

public class CompetencyLevelTaskMapping : Entity
{
    public Guid CompetencyLevelId { get; set; }
    public Guid TaskOfPeriodId { get; set; }
    public virtual CompetencyLevel CompetencyLevel { get; set; } = null!;
    public virtual TaskOfPeriod TaskOfPeriod { get; set; } = null!;
}