using System.ComponentModel;
using PerformanceManagementSystem.Data.Enums;

namespace PerformanceManagementSystem.Data.Views.Reports;

public class SelfReportListResponseDto
{
    public SelfReportListResponseDto()
    {
        Items = new List<SelfReportListItemDto>();
    }
    public Guid Id { get; set; }
    [DisplayName("وظیفه یا پروژه")]
    public string Title { get; set; } = null!;
    [DisplayName("میزان موفقیت در وظیفه")]
    public SuccessRateEnum SuccessRate { get; set; }
    [DisplayName("شرح کامل وظیفه")]
    public string? Description { get; set; }
    [DisplayName("موفقیت و اثرگذاری من در وظیفه")]
    public string? RoleAndInfluence { get; set; }
    [DisplayName("وضعیت")]
    public bool Active { get; set; }
    [DisplayName("آخرین بروزرسانی")]
    public DateTimeOffset UpdatedDate { get; set; }
    public IList<SelfReportListItemDto> Items { get; set; }
}

public class SelfReportListItemDto
{
    public Guid Id { get; set; }
    [DisplayName("موقعیت")]
    public bool Manager { get; set; }
    [DisplayName("ارزیابی کننده")]
    public string Fullname { get; set; } = null!;
    [DisplayName("میزان مسئولیت")]
    public DutyEnum Duty { get; set; }
}