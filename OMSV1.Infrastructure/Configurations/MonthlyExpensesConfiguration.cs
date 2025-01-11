using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMSV1.Domain.Entities.Expenses;

namespace OMSV1.Infrastructure.Configurations;

public class MonthlyExpensesConfiguration : IEntityTypeConfiguration<MonthlyExpenses>
{
    public void Configure(EntityTypeBuilder<MonthlyExpenses> builder)
    {
        builder.HasKey(me => me.Id);

        builder.Property(me => me.TotalAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(me => me.Notes)
            .HasMaxLength(500);

        builder.Property(me => me.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.HasOne(me => me.Office)
            .WithMany()
            .HasForeignKey(me => me.OfficeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(me => me.Governorate)
            .WithMany()
            .HasForeignKey(me => me.GovernorateId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(me => me.Profile)
            .WithMany()
            .HasForeignKey(me => me.ProfileId)
            .OnDelete(DeleteBehavior.Restrict);
            
        builder.Property(me => me.ThresholdId)
            .IsRequired(false);

        builder.HasOne(me => me.Threshold)
            .WithMany()
            .HasForeignKey(me => me.ThresholdId)
            .OnDelete(DeleteBehavior.Restrict);
            

        builder.Navigation(me => me.actions).HasField("_actions");
        builder.Navigation(me => me.dailyExpenses).HasField("_dailyExpenses");

        builder.ToTable("MonthlyExpenses");
    }
}
