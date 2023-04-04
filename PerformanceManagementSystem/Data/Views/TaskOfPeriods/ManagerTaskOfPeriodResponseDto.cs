using System.ComponentModel;

namespace PerformanceManagementSystem.Data.Views.TaskOfPeriods;

public class ManagerTaskOfPeriodResponseDto : BaseView
{
    public ManagerTaskOfPeriodResponseDto()
    {
        CompetencyNames = new List<CompetencyNames>();
    }

    public string TaskOwnerName { get; set; } = null!;
    [DisplayName("عنوان وظیفه یا پروژه")] 
    public new string Title { get; set; } = null!;
    [DisplayName("شرح کامل وظیفه")]
    public string? Description { get; set; } = null!;
    [DisplayName("موفقیت و اثرگذاری من در این وظیفه")]
    public string? RoleAndInfluence { get; set; }
    public IList<CompetencyNames> CompetencyNames { get; set; }
    public Guid? PositionId { get; set; }
}

public class CompetencyNames
{
    public string? CompetencyName { get; set; }
    public string? CompetencyLevelName { get; set; }
}