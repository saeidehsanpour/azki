using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PerformanceManagementSystem.Data.Models;

namespace PerformanceManagementSystem.Data.Configurations;

public class PerformanceManagementPeriodUserMappingsConfiguration : IEntityTypeConfiguration<PerformanceManagementPeriodUserMapping>
{
    public void Configure(EntityTypeBuilder<PerformanceManagementPeriodUserMapping> builder)
    {
        builder.HasQueryFilter(a => !a.Deleted);
    }
}