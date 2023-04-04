using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PerformanceManagementSystem.Data.Models;

namespace PerformanceManagementSystem.Data.Configurations;

public class AppUserClaimConfiguration : IEntityTypeConfiguration<AppUserClaim>
{
    public void Configure(EntityTypeBuilder<AppUserClaim> builder)
    {
        builder.ToTable("UserClaims");

        builder.HasOne(ur => ur.AppUser)
            .WithMany(r => r.UserClaims)
            .HasForeignKey(ur => ur.UserId)
            .IsRequired();
    }
}