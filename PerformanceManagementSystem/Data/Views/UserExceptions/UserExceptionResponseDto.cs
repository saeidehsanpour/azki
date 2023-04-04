using System.ComponentModel;

namespace PerformanceManagementSystem.Data.Views.UserExceptions;

public class UserExceptionResponseDto : BaseView
{
    public UserExceptionResponseDto()
    {
        Items = new List<UserExceptionItemResponseDto>();
    }
    [DisplayName("کارها، ویژگی ها و رفتارهایی که خوبه ادامه بدی")]
    public string? Continue { get; set; }
    public string Fullname { get; set; } = null!;
    public IList<UserExceptionItemResponseDto> Items { get; set; }
}

public class UserExceptionItemResponseDto
{
    [DisplayName("نقاط یا شایستگی قابل بهبود")]
    public string CompetencyName { get; set; } = null!;
    [DisplayName("اکشن و منابع پیشنهادی")]
    public string Description { get; set; } = null!;
}