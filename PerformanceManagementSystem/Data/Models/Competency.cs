namespace PerformanceManagementSystem.Data.Models;

public class Competency : Entity
{
    public Competency()
    {
        CompetencyLevels = new HashSet<CompetencyLevel>();
        UserExceptionCompetencyMappings = new HashSet<UserExceptionCompetencyMapping>();
    }
    public string? Description { get; set; }
    public Guid CompetencyCategoryId { get; set; }
    public virtual CompetencyCategory CompetencyCategory { get; set; } = null!;
    public virtual ICollection<CompetencyLevel> CompetencyLevels { get; set; }
    public virtual ICollection<UserExceptionCompetencyMapping> UserExceptionCompetencyMappings { get; set; }
}