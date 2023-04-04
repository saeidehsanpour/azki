using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PerformanceManagementSystem.Data.Views.Positions;

public class PositionRequestDto : BaseRequest
{
    [DisplayName("نام")]
    [Required(ErrorMessage = "نام مقام اجباری می باشد")]
    public string Title { get; set; } = null!;
    [DisplayName("وضعیت")]
    public bool Active { get; set; }
    public Guid OrganizationId { get; set; }
}