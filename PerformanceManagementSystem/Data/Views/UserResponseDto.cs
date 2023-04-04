using System.ComponentModel;

namespace PerformanceManagementSystem.Data.Views;

public class UserResponseDto : BaseView
{
    [DisplayName("نام")]
    public string? Name { get; set; }
    [DisplayName("ایمیل")]
    public string Email { get; set; } = null!;
}