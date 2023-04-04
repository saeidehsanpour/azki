using System.ComponentModel;

namespace PerformanceManagementSystem.Data.Views.Organizations;

public class OrganizationRequestDto : BaseRequest
{
    [DisplayName("نام")]
    public string Title { get; set; } = null!;
    [DisplayName("وضعیت")]
    public bool Active { get; set; }
    [DisplayName("ادمین")]
    public Guid AdminUserId { get; set; }
}