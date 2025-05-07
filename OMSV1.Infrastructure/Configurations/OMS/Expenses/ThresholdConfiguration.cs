using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMSV1.Domain.Entities.Expenses;

public class ThresholdConfiguration : IEntityTypeConfiguration<Threshold>
{
    public void Configure(EntityTypeBuilder<Threshold> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(t => t.MinValue)
            .IsRequired();

        builder.Property(t => t.MaxValue)
            .IsRequired();

        builder.ToTable("Thresholds");
    }
}
