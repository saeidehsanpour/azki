namespace PerformanceManagementSystem.Data.Views.CompetencyLevels;

public class CompetencyLevelSelectListResponseDto
{
    public Guid CompetencyId { get; set; }
    public string? CompetencyName { get; set; }
    public Guid CompetencyLevelId { get; set; }
    public string? CompetencyLevelName { get; set; }
}
