using PerformanceManagementSystem.Data.Enums;

namespace PerformanceManagementSystem.Data.Models;

public class TaskOfPeriod : Entity
{
    public Guid? MainTaskOfPeriodId { get; set; }
    public Guid? UserId { get; set; }
    public Guid PerformanceManagementPeriodUserMappingId { get; set; }
    public TaskType Type { get; set; }
    public string? Description { get; set; }
    public string? RoleAndInfluence { get; set; } // should start from manager to user
    public string? Continue { get; set; } // if user was manager, this field use for feedback
    public string? ShouldImprove { get; set; } // if user was manager, this field use for expectations
    public virtual AppUser? User { get; set; }
    public virtual TaskOfPeriod? MainTaskOfPeriod { get; set; }
    public virtual PerformanceManagementPeriodUserMapping PerformanceManagementPeriodUserMapping { get; set; } = null!;
    public SuccessRateEnum SuccessRate { get; set; }
    public DutyEnum DutyInTask { get; set; }
    public virtual ICollection<TaskUserMention> TaskUserMentions { get; set; }
    public virtual ICollection<CompetencyLevelTaskMapping> CompetencyLevelTaskMappings { get; set; }
    public TaskOfPeriod()
    {
        TaskUserMentions = new HashSet<TaskUserMention>();
        CompetencyLevelTaskMappings = new HashSet<CompetencyLevelTaskMapping>();
    }
}