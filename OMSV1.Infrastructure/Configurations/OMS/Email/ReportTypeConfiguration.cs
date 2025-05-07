using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMSV1.Domain.Entities.Reports;

namespace OMSV1.Infrastructure.Configurations
{
    public class ReportTypeConfiguration : IEntityTypeConfiguration<ReportType>
    {
        public void Configure(EntityTypeBuilder<ReportType> builder)
        {
            builder.HasKey(rt => rt.Id); // Primary Key

            builder.Property(rt => rt.Name)
                .IsRequired()
                .HasMaxLength(100); // Adjust as needed

            builder.Property(rt => rt.Description)
                .HasMaxLength(500); // Optional, allows nulls

            // Index for faster search
            builder.HasIndex(rt => rt.Name).IsUnique();

            // Table Mapping
            builder.ToTable("ReportTypes");
        }
    }
}
