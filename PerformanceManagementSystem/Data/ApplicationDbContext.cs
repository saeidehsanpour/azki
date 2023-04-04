using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PerformanceManagementSystem.Data.Models;
using System.Reflection;

namespace PerformanceManagementSystem.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid, AppUserClaim,
    AppUserRole, UserLogin,
    RoleClaim, UserToken>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<CompetencyCategory> CompetencyCategories { get; set; }
    public DbSet<Competency> Competencies { get; set; }
    public DbSet<CompetencyLevel> CompetencyLevels { get; set; }
    public DbSet<CompetencyLevelTaskMapping> CompetencyLevelTaskMappings { get; set; }
    public DbSet<CompetencyCategoryPositionMapping> CompetencyCategoryPositionMappings { get; set; }
    public DbSet<PerformanceManagementPeriod> PerformanceManagementPeriods { get; set; }
    public DbSet<PerformanceManagementPeriodUserMapping> PerformanceManagementPeriodUserMappings { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<TaskOfPeriod> TaskOfPeriods { get; set; }
    public DbSet<TaskUserMention> TaskUserMentions { get; set; }
    public DbSet<UserManagerMapping> UserManagerMappings { get; set; }
    public DbSet<ManagerEvaluation> ManagerEvaluations { get; set; }
    public DbSet<ManagerEvaluationQuestion> ManagerEvaluationQuestions { get; set; }
    public DbSet<ManagerEvaluationAnswer> ManagerEvaluationAnswers { get; set; }
    public DbSet<UserExceptionCompetencyMapping> UserExceptionCompetencyMappings { get; set; }
    public DbSet<UserException> UserExceptions { get; set; }
}