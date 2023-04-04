namespace PerformanceManagementSystem.Data.Models;

public class CompetencyCategoryPositionMapping : BaseEntity
{
    public Guid PositionId { get; set; }
    public Guid CompetencyCategoryId { get; set; }
    public virtual Position Position { get; set; } = null!;
    public virtual CompetencyCategory CompetencyCategory { get; set; } = null!;
}