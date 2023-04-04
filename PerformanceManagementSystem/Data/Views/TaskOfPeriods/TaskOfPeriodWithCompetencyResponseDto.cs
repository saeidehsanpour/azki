using System.ComponentModel;

namespace PerformanceManagementSystem.Data.Views.TaskOfPeriods;

public class TaskOfPeriodWithCompetencyResponseDto : BaseView
{
    [DisplayName("عنوان شایستگی")] 
    public new string Title { get; set; } = null!;

    [DisplayName("سطح شایستگی")]
    public string CompetencyLevel { get; set; } = null!;
}