using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMSV1.Domain.Entities.Expenses;

namespace OMSV1.Infrastructure.Configurations;

public class ExpenseTypeConfiguration : IEntityTypeConfiguration<ExpenseType>
{
    public void Configure(EntityTypeBuilder<ExpenseType> builder)
    {
        builder.HasKey(et => et.Id);

        builder.Property(et => et.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.ToTable("ExpenseTypes");
    }
}
