using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PerformanceManagementSystem.Data.Models;

namespace PerformanceManagementSystem.Data.Configurations;

public class UserExceptionCompetencyMappingsConfiguration : IEntityTypeConfiguration<UserExceptionCompetencyMapping>
{
    public void Configure(EntityTypeBuilder<UserExceptionCompetencyMapping> builder)
    {
        builder.HasKey(ur => new { ur.CompetencyId, ur.UserExceptionId });

        builder.HasOne(ur => ur.Competency)
            .WithMany(r => r.UserExceptionCompetencyMappings)
            .HasForeignKey(ur => ur.CompetencyId)
            .IsRequired();

        builder.HasOne(ur => ur.UserException)
            .WithMany(r => r.UserExceptionCompetencyMappings)
            .HasForeignKey(ur => ur.UserExceptionId)
            .IsRequired();
    }
}