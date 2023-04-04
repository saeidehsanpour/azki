namespace PerformanceManagementSystem.Data.Models;

public class CompetencyCategory : Entity
{
    public CompetencyCategory()
    {
        Competencies = new HashSet<Competency>();
        CompetencyCategoryPositionMappings = new HashSet<CompetencyCategoryPositionMapping>();
    }

    public string? Description { get; set; }
    public virtual ICollection<Competency> Competencies { get; set; }
    public virtual ICollection<CompetencyCategoryPositionMapping> CompetencyCategoryPositionMappings { get; set; }
}