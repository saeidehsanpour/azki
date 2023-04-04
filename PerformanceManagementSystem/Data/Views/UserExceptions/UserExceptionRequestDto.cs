using System.ComponentModel;

namespace PerformanceManagementSystem.Data.Views.UserExceptions;

public class UserExceptionRequestDto : BaseRequest
{
    public UserExceptionRequestDto()
    {
        Items = new List<UserExceptionItemRequestDto>();
    }
    [DisplayName("کارها، ویژگی ها و رفتارهایی که خوبه ادامه بدی")]
    public string? Continue { get; set; }
    public IList<UserExceptionItemRequestDto> Items { get; set; }
    public Guid PerformanceManagementPeriodUserMappingId { get; set; }
}

public class UserExceptionItemRequestDto
{
    public Guid CompetencyId { get; set; }
    [DisplayName("نقاط یا شایستگی قابل بهبود")]
    public string? CompetencyName { get; set; }
    [DisplayName("اکشن و منابع پیشنهادی")]
    public string? Description { get; set; }
    [DisplayName("فعال")]
    public bool Use { get; set; }
}