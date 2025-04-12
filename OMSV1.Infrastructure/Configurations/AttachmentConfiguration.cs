using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMSV1.Domain.Entities.Attachments;

namespace OMSV1.Infrastructure.Configurations;

public class AttachmentConfiguration : IEntityTypeConfiguration<AttachmentCU>
{
    public void Configure(EntityTypeBuilder<AttachmentCU> builder)
    {
        // Primary Key
        builder.HasKey(a => a.Id);

        // Properties
        builder.Property(a => a.FilePath)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(a => a.EntityType)
            .IsRequired()
            .HasConversion<string>() // Store as string in the database
            .HasColumnName("EntityType"); // Explicit column name

        builder.Property(a => a.EntityId)
            .IsRequired();

        // Filtered Indexes for PostgreSQL
        builder.HasIndex(a => a.EntityId)
            .HasDatabaseName("IX_AttachmentCU_EntityId_DamagedDevice")
            .HasFilter("\"EntityType\" = 'DamagedDevice'");

        builder.HasIndex(a => a.EntityId)
            .HasDatabaseName("IX_AttachmentCU_EntityId_DamagedPassport")
            .HasFilter("\"EntityType\" = 'DamagedPassport'");

        builder.HasIndex(a => a.EntityId)
            .HasDatabaseName("IX_AttachmentCU_EntityId_Lecture")
            .HasFilter("\"EntityType\" = 'Lecture'");

        builder.HasIndex(a => a.EntityId)
            .HasDatabaseName("IX_AttachmentCU_EntityId_Expense")
            .HasFilter("\"EntityType\" = 'Expense'");
                    builder.HasIndex(a => a.EntityId)
            .HasDatabaseName("IX_AttachmentCU_EntityId_Document")
            .HasFilter("\"EntityType\" = 'Document'");

        // General composite index for queries across EntityType and EntityId
        builder.HasIndex(a => new { a.EntityType, a.EntityId })
            .HasDatabaseName("IX_AttachmentCU_EntityType_EntityId");

        // Table Name
        builder.ToTable("AttachmentCUs");
    }
}
