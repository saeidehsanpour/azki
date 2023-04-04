using System.ComponentModel.DataAnnotations;

namespace PerformanceManagementSystem.Data.Enums;

public enum SuccessRateEnum
{
    [Display(Name = "خیلی ضعیف")]
    VeryWeak = 0,
    [Display(Name = "ضعیف")]
    Weak = 5,
    [Display(Name = "معمولی")]
    Normal = 10, // d in matrix
    [Display(Name = "خوب")]
    Good = 15,// c in matrix
    [Display(Name = "خیلی خوب")]
    VeryGood = 20,// b in matrix
    [Display(Name = "عالی")]
    Perfect = 25// a in matrix
}