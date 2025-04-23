// MinistryConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMSV1.Domain.Entities.Ministries;

namespace OMSV1.Infrastructure.Configurations
{
    public class MinistryConfiguration : IEntityTypeConfiguration<Ministry>
    {
        public void Configure(EntityTypeBuilder<Ministry> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(200);

            // One-to-many: a ministry may have many documents
            builder
                .HasMany(m => m.Documents)
                .WithOne(d => d.Ministry)
                .HasForeignKey(d => d.MinistryId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.ToTable("Ministries");
        }
    }
}
