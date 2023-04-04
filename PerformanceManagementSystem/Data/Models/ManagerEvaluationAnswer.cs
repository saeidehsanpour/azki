namespace PerformanceManagementSystem.Data.Models;

public class ManagerEvaluationAnswer
{
    public Guid ManagerEvaluationId { get; set; }
    public virtual ManagerEvaluation ManagerEvaluation { get; set; } = null!;
    public Guid ManagerEvaluationQuestionId { get; set; }
    public virtual ManagerEvaluationQuestion ManagerEvaluationQuestion { get; set; } = null!;
    public DateTimeOffset CreatedDate { get; set; }
    public int Answer { get; set; }

    public ManagerEvaluationAnswer()
    {
        CreatedDate = DateTimeOffset.UtcNow;
    }
}