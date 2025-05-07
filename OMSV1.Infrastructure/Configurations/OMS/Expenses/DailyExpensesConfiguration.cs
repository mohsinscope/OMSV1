using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMSV1.Domain.Entities.Expenses;

namespace OMSV1.Infrastructure.Configurations
{
    public class DailyExpensesConfiguration : IEntityTypeConfiguration<DailyExpenses>
    {
        public void Configure(EntityTypeBuilder<DailyExpenses> builder)
        {
            // Primary Key
            builder.HasKey(de => de.Id);

            // Scalar properties configuration
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

            // Foreign key to ExpenseType
            builder.HasOne(de => de.ExpenseType)
                .WithMany()
                .HasForeignKey(de => de.ExpenseTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Foreign key to MonthlyExpenses
            builder.HasOne(de => de.MonthlyExpenses)
                .WithMany(me => me.dailyExpenses)
                .HasForeignKey(de => de.MonthlyExpensesId)
                .OnDelete(DeleteBehavior.Cascade);

            // Self-referencing relationship configuration:
            builder.HasMany(de => de.SubExpenses)
                .WithOne(de => de.ParentExpense)
                .HasForeignKey(de => de.ParentExpenseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure ParentExpenseId as optional since not all DailyExpenses are children.
            builder.Property(de => de.ParentExpenseId)
                .IsRequired(false);

            builder.ToTable("DailyExpenses");
        }
    }
}
