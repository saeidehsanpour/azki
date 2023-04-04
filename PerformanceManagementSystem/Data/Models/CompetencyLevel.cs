
using System.ComponentModel;

namespace PerformanceManagementSystem.Data.Models;

public class CompetencyLevel : Entity
{
    public CompetencyLevel()
    {
        CompetencyLevelTaskMappings = new HashSet<CompetencyLevelTaskMapping>();
    }
    [DisplayName("توضیحات")]
    public string? Description { get; set; }
    public Guid CompetencyId { get; set; }
    [DisplayName("سطح")]
    public int Level { get; set; }
    public virtual Competency Competency { get; set; } = null!;
    public virtual ICollection<CompetencyLevelTaskMapping> CompetencyLevelTaskMappings { get; set; }
}