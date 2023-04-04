namespace PerformanceManagementSystem.Data.Views.ManagerEvaluations;

public class ManagerEvaluationQuestionRequestDto
{
    public Guid Id { get; set; }
    public string QuestionName { get; set; } = null!;
    public int DisplayOrder { get; set; }
    public int Answer { get; set; }
}