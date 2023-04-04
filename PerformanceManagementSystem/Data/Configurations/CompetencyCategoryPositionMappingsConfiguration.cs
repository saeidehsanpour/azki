using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PerformanceManagementSystem.Data.Models;

namespace PerformanceManagementSystem.Data.Configurations;

public class CompetencyCategoryPositionMappingsConfiguration : IEntityTypeConfiguration<CompetencyCategoryPositionMapping>
{
    public void Configure(EntityTypeBuilder<CompetencyCategoryPositionMapping> builder)
    {
        builder.HasKey(ur => new { ur.CompetencyCategoryId, ur.PositionId });

        builder.HasOne(ur => ur.Position)
            .WithMany(r => r.CompetencyCategoryPositionMappings)
            .HasForeignKey(ur => ur.PositionId)
            .IsRequired();

        builder.HasOne(ur => ur.CompetencyCategory)
            .WithMany(r => r.CompetencyCategoryPositionMappings)
            .HasForeignKey(ur => ur.CompetencyCategoryId)
            .IsRequired();
    }
}