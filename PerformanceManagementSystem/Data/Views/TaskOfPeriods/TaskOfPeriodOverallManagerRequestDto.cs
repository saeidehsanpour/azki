using System.ComponentModel;

namespace PerformanceManagementSystem.Data.Views.TaskOfPeriods;

public class TaskOfPeriodOverallManagerRequestDto : BaseRequest
{
    public TaskOfPeriodOverallManagerRequestDto()
    {
        CompetencyLevelTaskMappings = new List<CompetencyLevelTaskMappingRequestDto>();
    }

    public Guid PerformanceManagementPeriodUserMappingId { get; set; }
    [DisplayName("نظر کلی مدیر درمورد عملکرد فرد توی دوره در مجموع")]
    public string Description { get; set; } = null!;
    [DisplayName("کارها، ویژگی ها و رفتارهایی که خوبه ادامه بدی")]
    public string Continue { get; set; } = null!;
    [DisplayName("کارها و ویژگی هایی که باید بهبود پیدا کنن")]
    public string ShouldImprove { get; set; } = null!;
    [DisplayName("کارهای جدیدی که باید شروعشون کنی")]
    public string RoleAndInfluence { get; set; } = null!;
    public SuccessRateRequestEnum SuccessRate { get; set; }
    public IList<CompetencyLevelTaskMappingRequestDto> CompetencyLevelTaskMappings { get; set; }
}

public enum SuccessRateRequestEnum
{
    D = 10,
    C = 15,
    B = 20,
    A = 25
}
