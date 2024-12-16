using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DomainAction = OMSV1.Domain.Entities.Expenses.Action;

namespace OMSV1.Infrastructure.Configurations;
public class ActionConfiguration : IEntityTypeConfiguration<DomainAction>
{
    public void Configure(EntityTypeBuilder<DomainAction> builder)
    {
        builder.HasKey(a => a.Id); 

        builder.Property(a => a.Notes)
            .HasMaxLength(500)
            .IsRequired();

        builder.HasOne(a => a.Profile)
            .WithMany() 
            .HasForeignKey(a => a.ProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.MonthlyExpenses)
            .WithMany(me => me.actions)
            .HasForeignKey(a => a.MonthlyExpensesId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("Actions");
    }
}