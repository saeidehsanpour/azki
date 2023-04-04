using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PerformanceManagementSystem.Data.Models;

namespace PerformanceManagementSystem.Data.Configurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.Property(a => a.Firstname).HasMaxLength(50);
        builder.Property(a => a.Lastname).HasMaxLength(70);
        builder.ToTable(name: "Users");
    }
}