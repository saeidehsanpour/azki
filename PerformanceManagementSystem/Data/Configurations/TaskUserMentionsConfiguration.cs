using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PerformanceManagementSystem.Data.Models;

namespace PerformanceManagementSystem.Data.Configurations;

public class TaskUserMentionsConfiguration : IEntityTypeConfiguration<TaskUserMention>
{
    public void Configure(EntityTypeBuilder<TaskUserMention> builder)
    {
        builder.HasKey(ur => new { ur.UserId, ur.TaskOfPeriodId });

        builder.HasOne(ur => ur.TaskOfPeriod)
            .WithMany(r => r.TaskUserMentions)
            .HasForeignKey(ur => ur.TaskOfPeriodId)
            .IsRequired();

        builder.HasOne(ur => ur.User)
            .WithMany(r => r.TaskUserMentions)
            .HasForeignKey(ur => ur.UserId)
            .IsRequired();
    }
}