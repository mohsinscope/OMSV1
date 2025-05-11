using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMSV1.Domain.Entities.Attachments;
using OMSV1.Domain.Enums;

namespace OMSV1.Infrastructure.Configurations
{
    public class DocumentAttachmentConfiguration : IEntityTypeConfiguration<DocumentAttachment>
    {
        public void Configure(EntityTypeBuilder<DocumentAttachment> builder)
        {
            /* ─────────────── Table & PK ─────────────── */
            builder.ToTable("DocumentAttachments");
            builder.HasKey(a => a.Id);

            /* ─────────────── Properties ─────────────── */
            builder.Property(a => a.FilePath)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(a => a.DocumentId)
                   .IsRequired();

            // نحفظ Enum كـ string لتسهيل الفلترة
            builder.Property(a => a.EntityType)
                   .HasConversion<string>()                // ← يحفظ باسم 'Document'
                   .HasDefaultValue(EntityType.Document)   // ← ضمان القيمة الافتراضية
                   .IsRequired();

            /* ─────────────── Relationships ─────────────── */
            builder.HasOne(a => a.Document)
                   .WithMany(d => d.DocumentAttachments)
                   .HasForeignKey(a => a.DocumentId)
                   .OnDelete(DeleteBehavior.Cascade);

            /* ─────────────── Indexes ─────────────── */
            builder.HasIndex(a => a.DocumentId)
                   .HasDatabaseName("IX_DocumentAttachments_DocumentId");

            // فهرس مركّب يضمن أن السجلات دائماً EntityType = 'Document'
            builder.HasIndex(a => new { a.EntityType, a.DocumentId })
                   .HasDatabaseName("IX_DocAttachments_EntityType_DocumentId")
                   .HasFilter("\"EntityType\" = 'Document'"); // لـ PostgreSQL
        }
    }
}
