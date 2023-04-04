using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using PerformanceManagementSystem.Data.Enums;

namespace PerformanceManagementSystem.Data.Views.TaskOfPeriods;

public class PeerTaskOfPeriodRequestDto : BaseRequest
{
    public PeerTaskOfPeriodRequestDto()
    {
        CompetencyLevelIds = new List<Guid>();
    }
    [DisplayName("نقش من در این وظیفه یا پروژه")]
    public DutyEnum DutyInTask { get; set; }
    [DisplayName("میزان موفقیت ارزیابی شونده")]
    public SuccessRateEnum SuccessRate { get; set; }
    [Required(ErrorMessage = "اجباری")]
    [DisplayName("توضیحات من درمورد نقش و اثرگذاری من در این وظیفه یا پروژه")]
    public string? RoleAndInfluence { get; set; }
    [DisplayName("فرد از این شایستگی در این وظیفه یا پروژه در سطح زیر استفاده کرده")]
    public List<Guid> CompetencyLevelIds { get; set; }
    public Guid MainTaskOfPeriodId { get; set; }
}