using System.ComponentModel;

namespace PerformanceManagementSystem.Data.Models;


public class BaseEntity
{
    [DisplayName("نام")]
    public string Title { get; set; } = null!;
    [DisplayName("تاریخ ایجاد")]
    public DateTimeOffset CreatedDate { get; set; }
    [DisplayName("آخرین بروز رسانی")]
    public DateTimeOffset UpdatedDate { get; set; }
    [DisplayName("حذف شده")]
    public bool Deleted { get; set; }
    [DisplayName("وضعیت")]
    public bool Active { get; set; }

    public BaseEntity()
    {
        CreatedDate = DateTimeOffset.UtcNow;
        UpdatedDate = DateTimeOffset.UtcNow;
    }
}

public class Entity : BaseEntity
{
    public Guid Id { get; set; }
}