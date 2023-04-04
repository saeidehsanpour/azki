using PerformanceManagementSystem.Data.Enums;
using System.ComponentModel;

namespace PerformanceManagementSystem.Data.Views.TaskOfPeriods;

public class PeerTaskOfPeriodListResponseDto : BaseView
{
    public Guid MainTaskId { get; set; }
    [DisplayName("عنوان")] 
    public new string Title { get; set; } = null!;
    [DisplayName("همکار")]
    public string PeerName { get; set; } = null!;
    [DisplayName("موفقیت")]
    public SuccessRateEnum? SuccessRate { get; set; }
    [DisplayName("تسلط")]
    public DutyEnum? DutyInTask { get; set; }
}