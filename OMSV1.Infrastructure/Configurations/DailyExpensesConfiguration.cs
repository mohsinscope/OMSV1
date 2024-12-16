using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMSV1.Domain.Entities.Expenses;

namespace OMSV1.Infrastructure.Configurations;

public class DailyExpensesConfiguration : IEntityTypeConfiguration<DailyExpenses>
{
    public void Configure(EntityTypeBuilder<DailyExpenses> builder)
    {
        builder.HasKey(de => de.Id);

        builder.Property(de => de.Price)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(de => de.Quantity)
            .IsRequired();

        builder.Property(de => de.Amount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(de => de.Notes)
            .HasMaxLength(500);

        builder.Property(de => de.ExpenseDate)
            .IsRequired();

        builder.HasOne(de => de.ExpenseType)
            .WithMany()
            .HasForeignKey(de => de.ExpenseTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(de => de.MonthlyExpenses)
            .WithMany(me => me.dailyExpenses)
            .HasForeignKey(de => de.MonthlyExpensesId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("DailyExpenses");
    }
}