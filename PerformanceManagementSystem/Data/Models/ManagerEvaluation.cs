namespace PerformanceManagementSystem.Data.Models;

public class ManagerEvaluation : Entity
{
    public Guid PerformanceManagementPeriodUserMappingId { get; set; }
    public Guid ManagerId { get; set; }
    public string Continue { get; set; } = null!;
    public string ShouldImprove { get; set; } = null!;
    public virtual PerformanceManagementPeriodUserMapping PerformanceManagementPeriodUserMapping { get; set; } = null!;
    public virtual AppUser Manager { get; set; } = null!;
    public virtual ICollection<ManagerEvaluationAnswer> ManagerEvaluationAnswers { get; set; }

    public ManagerEvaluation()
    {
        ManagerEvaluationAnswers = new HashSet<ManagerEvaluationAnswer>();
    }
}