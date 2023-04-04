using PerformanceManagementSystem.Data.Enums;
using PerformanceManagementSystem.Data.Views.CompetencyLevels;
using PerformanceManagementSystem.Data.Views.TaskOfPeriods;
using System.ComponentModel;

namespace PerformanceManagementSystem.Data.Views.Reports;

public class SelfReportManagerOverallTaskOfPeriodDetailResponseDto
{
    public SelfReportManagerOverallTaskOfPeriodDetailResponseDto()
    {
        CompetencyLevels = new List<CompetencyLevelSelectListResponseDto>();
    }

    public Guid Id { get; set; }
    public string Fullname { get; set; } = null!;
    [DisplayName("نظر کلی مدیر درمورد عملکرد فرد توی دوره در مجموع")]
    public string? Description { get; set; } = null!;
    [DisplayName("کارها، ویژگی ها و رفتارهایی که خوبه ادامه بدی")]
    public string? Continue { get; set; } = null!;
    [DisplayName("کارها و ویژگی هایی که باید بهبود پیدا کنن")]
    public string? ShouldImprove { get; set; } = null!;
    [DisplayName("کارهای جدیدی که باید شروعشون کنی")]
    public string? RoleAndInfluence { get; set; } = null!;
    public SuccessRateRequestEnum SuccessRate { get; set; }
    public IList<CompetencyLevelSelectListResponseDto> CompetencyLevels { get; set; }
}