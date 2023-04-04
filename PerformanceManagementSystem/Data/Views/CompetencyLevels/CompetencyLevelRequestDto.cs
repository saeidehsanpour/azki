using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PerformanceManagementSystem.Data.Views.CompetencyLevels;

public class CompetencyLevelRequestDto : BaseRequest
{
    [DisplayName("نام")]
    [Required(ErrorMessage = "نام شایستگی اجباری می باشد")]
    public string Title { get; set; } = null!;
    [DisplayName("وضعیت")]
    public bool Active { get; set; }
    [DisplayName("توضیحات")]
    public string? Description { get; set; }
    public Guid CompetencyId { get; set; }
    [DisplayName("سطح")]
    public int Level { get; set; }
}