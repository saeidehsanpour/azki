using System.ComponentModel;

namespace PerformanceManagementSystem.Data.Views.CompetencyCategories;

public class CompetencyCategoryResponseDto : BaseView
{
    [DisplayName("نام")]
    public string Title { get; set; } = null!;
    [DisplayName("وضعیت")]
    public bool Active { get; set; }
    [DisplayName("توضیحات")]
    public string? Description { get; set; }
}