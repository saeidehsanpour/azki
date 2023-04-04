using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PerformanceManagementSystem.Data.Models;

namespace PerformanceManagementSystem.Data.Configurations;

public class CompetencyCategoryConfiguration : IEntityTypeConfiguration<CompetencyCategory>
{
    public void Configure(EntityTypeBuilder<CompetencyCategory> builder)
    {
        builder.HasQueryFilter(a => !a.Deleted);
    }
}