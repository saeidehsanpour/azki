using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PerformanceManagementSystem.Data.Views.CompetencyCategories;

public class CompetencyCategoryRequestDto : BaseRequest
{
    [DisplayName("نام")]
    [Required(ErrorMessage = "نام دسته بندی اجباری می باشد")]
    public string Title { get; set; } = null!;
    [DisplayName("وضعیت")]
    public bool Active { get; set; }
    [DisplayName("توضیحات")]
    public string? Description { get; set; }
}