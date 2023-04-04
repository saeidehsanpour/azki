using System.ComponentModel;

namespace PerformanceManagementSystem.Data.Views.Organizations;

public class OrganizationResponseDto : BaseView
{
    [DisplayName("نام")]
    public string Title { get; set; } = null!;
    [DisplayName("وضعیت")]
    public bool Active { get; set; }
    [DisplayName("ادمین")]
    public string AdminUserName { get; set; } = null!;
    public Guid AdminUserId { get; set; }
}