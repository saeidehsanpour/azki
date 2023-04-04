using PerformanceManagementSystem.Data.Enums;
using PerformanceManagementSystem.Data.Views.CompetencyLevels;

namespace PerformanceManagementSystem.Data.Views.Reports;

public class SelfReportTaskOfPeriodDetailResponseDto
{
    public SelfReportTaskOfPeriodDetailResponseDto()
    {
        CompetencyLevels = new List<CompetencyLevelSelectListResponseDto>();
    }

    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Fullname { get; set; } = null!;
    public SuccessRateEnum SuccessRate { get; set; }
    public DutyEnum Duty { get; set; }
    public string Description { get; set; } = null!;
    public IList<CompetencyLevelSelectListResponseDto> CompetencyLevels { get; set; }
}