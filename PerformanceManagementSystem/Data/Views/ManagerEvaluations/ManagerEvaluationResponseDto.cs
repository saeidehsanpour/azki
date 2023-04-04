using System.ComponentModel;

namespace PerformanceManagementSystem.Data.Views.ManagerEvaluations;

public class ManagerEvaluationResponseDto
{
    public ManagerEvaluationResponseDto()
    {
        ManagerEvaluationAverageOfAnswers = new List<ManagerEvaluationAverageOfAnswerResponseDto>();
        ManagerEvaluationStringQuestions = new List<ManagerEvaluationStringQuestionResponseDto>();
    }

    public IList<ManagerEvaluationStringQuestionResponseDto> ManagerEvaluationStringQuestions { get; set; }
    public IList<ManagerEvaluationAverageOfAnswerResponseDto> ManagerEvaluationAverageOfAnswers { get; set; }
}

public class ManagerEvaluationAverageOfAnswerResponseDto
{
    public ManagerEvaluationAverageOfAnswerResponseDto()
    {
        Answers = new List<int>();
    }
    public double Average { get; set; }
    public string? QuestionName { get; set; }
    public List<int> Answers { get; set; }
}

public class ManagerEvaluationStringQuestionResponseDto
{
    [DisplayName("ویژگی ها و رفتارهایی که خوبه اونارو ادامه بده")]
    public string Continue { get; set; } = null!;
    [DisplayName("ویژگی‌ها و رفتارهایی که خوبه شروع کنه یا متفاوت انجام بده تا اثرگذاری بیشتری داشته باشه")]
    public string ShouldImprove { get; set; } = null!;
}