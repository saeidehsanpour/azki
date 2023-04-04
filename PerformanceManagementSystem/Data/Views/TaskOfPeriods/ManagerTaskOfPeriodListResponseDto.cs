using PerformanceManagementSystem.Data.Enums;
using System.ComponentModel;

namespace PerformanceManagementSystem.Data.Views.TaskOfPeriods;

public class ManagerTaskOfPeriodListResponseDto
{
    public ManagerTaskOfPeriodListResponseDto()
    {
        Items = new List<ManagerTaskOfPeriodItemResponseDto>();
    }
    public Guid UserId { get; set; }
    [DisplayName("همکار")]
    public string Fullname { get; set; } = null!;
    public IList<ManagerTaskOfPeriodItemResponseDto> Items { get; set; }
}


public class ManagerTaskOfPeriodItemResponseDto
{
    [DisplayName("عنوان")]
    public string Title { get; set; } = null!;
    public Guid Id { get; set; }
    public Guid MainTaskId { get; set; }
    [DisplayName("موفقیت")]
    public SuccessRateEnum? SuccessRate { get; set; }
    [DisplayName("تسلط")]
    public DutyEnum? DutyInTask { get; set; }
}

public class ManagerTaskOfPeriodForMappingResponseDto
{
    public string Title { get; set; } = null!;
    public Guid Id { get; set; }
    public Guid MainTaskId { get; set; }
    public SuccessRateEnum? SuccessRate { get; set; }
    public DutyEnum? DutyInTask { get; set; }
    public Guid UserId { get; set; }
    public string Fullname { get; set; } = null!;
}