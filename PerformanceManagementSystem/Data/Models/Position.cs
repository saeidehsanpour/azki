namespace PerformanceManagementSystem.Data.Models;

public class Position : Entity
{
    public Position()
    {
        Users = new HashSet<AppUser>();
        CompetencyCategoryPositionMappings = new HashSet<CompetencyCategoryPositionMapping>();
    }

    public Guid OrganizationId { get; set; }
    public Organization Organization { get; set; } = null!;
    public virtual ICollection<AppUser> Users { get; set; }
    public virtual ICollection<CompetencyCategoryPositionMapping> CompetencyCategoryPositionMappings { get; set; }
}