using System.ComponentModel;

namespace PerformanceManagementSystem.Data.Views;

public class BaseView
{
    public Guid Id { get; set; }
    [DisplayName("نام")]
    public string Title { get; set; }
    [DisplayName("وضعیت")]
    public bool Active { get; set; }
    [DisplayName("آخرین بروزرسانی")]
    public DateTimeOffset UpdatedDate { get; set; }
}