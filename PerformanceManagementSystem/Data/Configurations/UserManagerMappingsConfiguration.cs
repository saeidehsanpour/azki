using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PerformanceManagementSystem.Data.Models;

namespace PerformanceManagementSystem.Data.Configurations;

public class UserManagerMappingsConfiguration : IEntityTypeConfiguration<UserManagerMapping>
{
    public void Configure(EntityTypeBuilder<UserManagerMapping> builder)
    {
        builder.HasKey(ur => new { ur.UserId, ur.ManagerId });

        builder.HasOne(ur => ur.User)
            .WithMany(r => r.Managers)
            .HasForeignKey(ur => ur.UserId)
            .IsRequired();

        builder.HasOne(ur => ur.Manager)
            .WithMany(r => r.Users)
            .HasForeignKey(ur => ur.ManagerId)
            .IsRequired();
    }
}