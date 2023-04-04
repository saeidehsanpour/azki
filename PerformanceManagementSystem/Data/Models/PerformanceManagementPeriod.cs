namespace PerformanceManagementSystem.Data.Models;

public class PerformanceManagementPeriod : Entity
{
    public PerformanceManagementPeriod()
    {
        PerformanceManagementPeriodUserMappings = new HashSet<PerformanceManagementPeriodUserMapping>();
    }
    public Guid OrganizationId { get; set; }
    public virtual Organization Organization { get; set; } = null!;
    public DateTimeOffset SelfScoreStartDate { get; set; }
    public DateTimeOffset SelfScoreEndDate { get; set; }
    public DateTimeOffset OtherScoreStartDate { get; set; }
    public DateTimeOffset OtherScoreEndDate { get; set; }
    public DateTimeOffset ManagerScoreStartDate { get; set; }
    public DateTimeOffset ManagerScoreEndDate { get; set; }
    public DateTimeOffset ReportStartDate { get; set; }
    public DateTimeOffset ReportEndDate { get; set; }
    public DateTimeOffset ExceptionScoreStartDate { get; set; }
    public DateTimeOffset ExceptionScoreEndDate { get; set; }
    public virtual ICollection<PerformanceManagementPeriodUserMapping> PerformanceManagementPeriodUserMappings { get; set; }
}