using System.ComponentModel;

namespace PerformanceManagementSystem.Data.Views.Competencies;

public class CompetencyResponseDto : BaseView
{
    [DisplayName("توضیحات")]
    public string? Description { get; set; }
    [DisplayName("عنوان شایستگی")] 
    public new string Title { get; set; } = null!;
    public Guid CompetencyCategoryId { get; set; }
}