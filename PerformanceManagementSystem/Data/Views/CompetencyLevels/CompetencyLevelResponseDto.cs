using System.ComponentModel;

namespace PerformanceManagementSystem.Data.Views.CompetencyLevels;

public class CompetencyLevelResponseDto : BaseView
{
    [DisplayName("توضیحات")]
    public string? Description { get; set; }
    public Guid CompetencyId { get; set; }
    [DisplayName("سطح")]
    public int Level { get; set; }
}