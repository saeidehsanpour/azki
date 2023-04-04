using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PerformanceManagementSystem.Data.Models;

namespace PerformanceManagementSystem.Data.Configurations;

public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.HasQueryFilter(a => !a.Deleted);
        builder.Property(a => a.Title).HasMaxLength(50);
    }
}