namespace PerformanceManagementSystem.Data.Models;

public class UserException : Entity
{
    public Guid PerformanceManagementPeriodUserMappingId { get; set; }
    public Guid ManagerId { get; set; }
    public string? Continue { get; set; }
    public virtual PerformanceManagementPeriodUserMapping PerformanceManagementPeriodUserMapping { get; set; } = null!;
    public virtual AppUser Manager { get; set; } = null!;
    public virtual ICollection<UserExceptionCompetencyMapping> UserExceptionCompetencyMappings { get; set; }

    public UserException()
    {
        UserExceptionCompetencyMappings = new HashSet<UserExceptionCompetencyMapping>();
    }
}