using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMSV1.Domain.Entities.Reports;

namespace OMSV1.Infrastructure.Configurations
{
    public class EmailReportConfiguration : IEntityTypeConfiguration<EmailReport>
    {
        public void Configure(EntityTypeBuilder<EmailReport> builder)
        {
            builder.HasKey(er => er.Id); // Primary Key

            builder.Property(er => er.FullName)
                .IsRequired()
                .HasMaxLength(150); // Adjust as needed

            builder.Property(er => er.Email)
                .IsRequired()
                .HasMaxLength(250); // Adjust as needed

            // Unique index on Email to prevent duplicates
            builder.HasIndex(er => er.Email).IsUnique();

            // Many-to-Many Relationship with ReportType
            builder.HasMany(er => er.ReportTypes)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "EmailReportReportType",
                    j => j.HasOne<ReportType>().WithMany().HasForeignKey("ReportTypeId"),
                    j => j.HasOne<EmailReport>().WithMany().HasForeignKey("EmailReportId"),
                    j =>
                    {
                        j.HasKey("EmailReportId", "ReportTypeId");
                        j.ToTable("EmailReportReportTypes");
                    });

            // Table Mapping
            builder.ToTable("EmailReports");
        }
    }
}
