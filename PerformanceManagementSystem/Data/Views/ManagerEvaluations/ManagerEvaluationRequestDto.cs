using System.ComponentModel;

namespace PerformanceManagementSystem.Data.Views.ManagerEvaluations;

public class ManagerEvaluationRequestDto
{
    public ManagerEvaluationRequestDto()
    {
        ManagerEvaluationAnswerRequests = new List<ManagerEvaluationAnswerRequestDto>();
    }

    public Guid Id { get; set; }
    public Guid ManagerId { get; set; }
    public Guid PerformanceManagementPeriodUserMappingId { get; set; }
    [DisplayName("ویژگی ها و رفتارهایی که خوبه اونارو ادامه بده")]
    public string Continue { get; set; } = null!;
    [DisplayName("ویژگی‌ها و رفتارهایی که خوبه شروع کنه یا متفاوت انجام بده تا اثرگذاری بیشتری داشته باشه")]
    public string ShouldImprove { get; set; } = null!;
    public IList<ManagerEvaluationAnswerRequestDto> ManagerEvaluationAnswerRequests { get; set; }
}

public class ManagerEvaluationAnswerRequestDto
{
    public Guid ManagerEvaluationQuestionId { get; set; }
    public int Answer { get; set; }
    public string? QuestionName { get; set; }
}