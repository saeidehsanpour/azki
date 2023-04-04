using PerformanceManagementSystem.Data.Enums;
using System.ComponentModel;

namespace PerformanceManagementSystem.Data.Views.TaskOfPeriods;

public class TaskOfPeriodListResponseDto : BaseView
{
    [DisplayName("عنوان")] 
    public new string Title { get; set; } = null!;
    [DisplayName("تعداد شایستگی های لینک شده")]
    public int NumberOfCompetencyLinked { get; set; }
    [DisplayName("میزان موفقیت")]
    public SuccessRateEnum SuccessRate { get; set; }
    [DisplayName("همکاران")]
    public int NumberOfMentionUsers { get; set; }
}