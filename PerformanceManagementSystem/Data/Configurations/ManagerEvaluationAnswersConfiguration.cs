using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PerformanceManagementSystem.Data.Models;

namespace PerformanceManagementSystem.Data.Configurations;

public class ManagerEvaluationAnswersConfiguration : IEntityTypeConfiguration<ManagerEvaluationAnswer>
{
    public void Configure(EntityTypeBuilder<ManagerEvaluationAnswer> builder)
    {
        builder.HasKey(ur => new { ur.ManagerEvaluationId, ur.ManagerEvaluationQuestionId });

        builder.HasOne(ur => ur.ManagerEvaluation)
            .WithMany(r => r.ManagerEvaluationAnswers)
            .HasForeignKey(ur => ur.ManagerEvaluationId)
            .IsRequired();

        builder.HasOne(ur => ur.ManagerEvaluationQuestion)
            .WithMany(r => r.ManagerEvaluationAnswers)
            .HasForeignKey(ur => ur.ManagerEvaluationQuestionId)
            .IsRequired();
    }
}