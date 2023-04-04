using System.ComponentModel.DataAnnotations;

namespace PerformanceManagementSystem.Data.Enums;

public enum DutyEnum
{
    SelfTaskWriter = 0,
    [Display(Name = "فرد اصلی وظیفه")]
    OwnerOfTask = 5,
    [Display(Name = "همکار در وظیفه")]
    Teammate = 10,
    [Display(Name = "مطلع و آگاه از وظیفه")]
    Informed = 15
}

public enum DutyEnumRequest
{
    [Display(Name = "فرد اصلی وظیفه")]
    OwnerOfTask = 5,
    [Display(Name = "همکار در وظیفه")]
    Teammate = 10,
    [Display(Name = "مطلع و آگاه از وظیفه")]
    Informed = 15
}