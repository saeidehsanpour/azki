namespace PerformanceManagementSystem.Data.Models;

public class ManagerEvaluationQuestion : Entity
{
    public ManagerEvaluationQuestion()
    {
        ManagerEvaluationAnswers = new HashSet<ManagerEvaluationAnswer>();
    }
    public int DisplayOrder { get; set; }
    public string Question { get; set; } = null!;
    public virtual ICollection<ManagerEvaluationAnswer> ManagerEvaluationAnswers { get; set; }
}