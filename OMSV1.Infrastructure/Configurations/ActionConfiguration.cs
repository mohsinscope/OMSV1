using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DomainAction = OMSV1.Domain.Entities.Expenses.Action;

namespace OMSV1.Infrastructure.Configurations;

public class ActionConfiguration : IEntityTypeConfiguration<DomainAction>
{
    public void Configure(EntityTypeBuilder<DomainAction> builder)
     {
         // Define the primary key
         builder.HasKey(a => a.Id);

         // Configure properties
         builder.Property(a => a.ActionType)
            .IsRequired()
            .HasMaxLength(100); // Adjust length as needed

         builder.HasIndex(a => a.ActionType);

         builder.Property(a => a.Notes)
            .HasMaxLength(500)
            .IsRequired();

         // Configure relationships
         builder.HasOne(a => a.Profile)
            .WithMany() // If Profile has a navigation property for actions, adjust this
            .HasForeignKey(a => a.ProfileId)
            .OnDelete(DeleteBehavior.Restrict);

         builder.HasOne(a => a.MonthlyExpenses)
            .WithMany(me => me.actions)
            .HasForeignKey(a => a.MonthlyExpensesId)
            .OnDelete(DeleteBehavior.Cascade);

         // Map to table
         builder.ToTable("Actions");
     }
}
