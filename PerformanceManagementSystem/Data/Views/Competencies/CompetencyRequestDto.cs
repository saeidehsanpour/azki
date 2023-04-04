using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PerformanceManagementSystem.Data.Views.Competencies;

public class CompetencyRequestDto : BaseRequest
{
    [DisplayName("نام")]
    [Required(ErrorMessage = "نام شایستگی اجباری می باشد")]
    public string Title { get; set; } = null!;
    [DisplayName("وضعیت")]
    public bool Active { get; set; }
    [DisplayName("توضیحات")]
    public string? Description { get; set; }
    public Guid CompetencyCategoryId { get; set; }
}