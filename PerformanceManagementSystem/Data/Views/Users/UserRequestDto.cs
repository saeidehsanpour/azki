using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PerformanceManagementSystem.Data.Views.Users;

public class UserRequestDto
{
    public UserRequestDto()
    {
        ManagerIds = new List<Guid>();
    }
    public Guid Id { get; set; }
    [DisplayName("نام")]
    [Required(ErrorMessage = "نام اجباری می باشد")]
    public string Firstname { get; set; } = null!;
    [DisplayName("نام خانوادگی")]
    [Required(ErrorMessage = "نام خانوادگی اجباری می باشد")]
    public string Lastname { get; set; } = null!;
    [DisplayName("رمز عبور")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "رمز عبور اجباری می باشد")]
    public string Password { get; set; } = null!;
    [DisplayName("پوزیشن")]
    public Guid? PositionId { get; set; }
    [DisplayName("نام کاربری")]
    [Required(ErrorMessage = "نام کاربری اجباری می باشد")]
    [EmailAddress(ErrorMessage = "ایمیل را بدرستی وارد نمایید.")]
    public string Username { get; set; } = null!;
    public List<Guid> ManagerIds { get; set; }
}

public class EditUserRequestDto
{
    public EditUserRequestDto()
    {
        ManagerIds = new List<Guid>();
    }
    public Guid Id { get; set; }
    [DisplayName("نام")]
    [Required(ErrorMessage = "نام اجباری می باشد")]
    public string Firstname { get; set; } = null!;
    [DisplayName("نام خانوادگی")]
    [Required(ErrorMessage = "نام خانوادگی اجباری می باشد")]
    public string Lastname { get; set; } = null!;
    [DisplayName("پوزیشن")]
    public Guid? PositionId { get; set; }
    public List<Guid> ManagerIds { get; set; }
}