using System.ComponentModel;

namespace PerformanceManagementSystem.Data.Views.TaskOfPeriods;

public class TaskOfPeriodOverallRequestDto : BaseRequest
{
    public TaskOfPeriodOverallRequestDto()
    {
        CompetencyLevelTaskMappings = new List<CompetencyLevelTaskMappingRequestDto>();
    }

    public Guid PerformanceManagementPeriodUserMappingId { get; set; }
    [DisplayName("منابع و ... که نیاز دارم تا کارم رو بهتر انجام بدم")]
    public string Description { get; set; } = null!;
    [DisplayName("ویژگی ها و رفتارهایی که خوب انجام دادم و می خوام ادامه بدم")]
    public string Continue { get; set; } = null!;
    [DisplayName("ویژگی ها و رفتارهایی که خوبه شروع کنم یا متفاوت انجام بدم تا اثرگذاری بیشتری داشته باشم")]
    public string ShouldImprove { get; set; } = null!;

    public IList<CompetencyLevelTaskMappingRequestDto> CompetencyLevelTaskMappings { get; set; }
}

public class CompetencyLevelTaskMappingRequestDto
{
    [DisplayName("سطح شایستگی")]
    public Guid CompetencyLevelId { get; set; }
    [DisplayName("دلیل انتخاب")]
    public string? Description { get; set; }
}
