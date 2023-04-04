using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using PerformanceManagementSystem.Data.Enums;

namespace PerformanceManagementSystem.Data.Views.TaskOfPeriods;

public class TaskOfPeriodRequestDto : BaseRequest
{
    public TaskOfPeriodRequestDto()
    {
        CompetencyLevelIds = new List<Guid>();
        TaskUserMentionIds = new List<Guid>();
    }
    [Required(ErrorMessage = "اجباری")]
    [DisplayName("عنوان وظیفه یا پروژه")]
    public string Title { get; set; } = null!;
    [DisplayName("عملکرد من در این پروژه")]
    public SuccessRateEnum SuccessRate { get; set; }
    [Required(ErrorMessage = "اجباری")]
    [DisplayName("شرح کامل وظیفه یا پروژه")]
    public string Description { get; set; } = null!;
    [Required(ErrorMessage = "اجباری")]
    [DisplayName("نقش و اثرگذاری من در این وظیفه یا پروژه")]
    public string? RoleAndInfluence { get; set; }
    [DisplayName("از این شایستگی در این وظیفه یا پروژه در سطح زیر استفاده کردم")]
    public List<Guid> CompetencyLevelIds { get; set; }
    [DisplayName("همکاران")]
    public List<Guid> TaskUserMentionIds { get; set; }
    public Guid PerformanceManagementPeriodUserMappingId { get; set; }
}