using PerformanceManagementSystem.Data.Views.CompetencyLevels;
using System.ComponentModel;

namespace PerformanceManagementSystem.Data.Views.Competencies;

public class CompetencyWithLevelsResponseDto : BaseView
{
    public CompetencyWithLevelsResponseDto()
    {
        Levels = new List<CompetencyLevelResponseDto>();
    }
    [DisplayName("توضیحات")]
    public string? Description { get; set; }
    [DisplayName("عنوان شایستگی")] 
    public new string Title { get; set; } = null!;
    public IList<CompetencyLevelResponseDto> Levels { get; set; }
}
