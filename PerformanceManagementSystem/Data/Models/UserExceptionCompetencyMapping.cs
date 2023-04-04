namespace PerformanceManagementSystem.Data.Models;

public class UserExceptionCompetencyMapping
{
    public Guid CompetencyId { get; set; }
    public virtual Competency Competency { get; set; } = null!;
    public Guid UserExceptionId { get; set; }
    public virtual UserException UserException { get; set; } = null!;
    public DateTimeOffset CreatedDate { get; set; }
    public string? Description { get; set; }

    public UserExceptionCompetencyMapping()
    {
        CreatedDate = DateTimeOffset.UtcNow;
    }
}