using System.ComponentModel;

namespace PerformanceManagementSystem.Data.Views.TaskOfPeriods;

public class PeerTaskOfPeriodResponseDto : BaseView
{
    public PeerTaskOfPeriodResponseDto()
    {
        CompetencyNames = new List<string>();
    }

    public string TaskOwnerName { get; set; } = null!;
    [DisplayName("عنوان وظیفه یا پروژه")] 
    public new string Title { get; set; } = null!;
    [DisplayName("شرح کامل وظیفه")]
    public string? Description { get; set; } = null!;
    [DisplayName("موفقیت و اثرگذاری من در این وظیفه")]
    public string? RoleAndInfluence { get; set; }
    public IList<string> CompetencyNames { get; set; }
    public Guid? PositionId { get; set; }
}