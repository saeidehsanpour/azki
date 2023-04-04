using System.ComponentModel;

namespace PerformanceManagementSystem.Data.Views.Users;

public class ManagerResponseDto
{
    public Guid Id { get; set; }
    [DisplayName("نام")]
    public string Fullname { get; set; } = null!;
    [DisplayName("مسئولیت")]
    public string? Position { get; set; }
}