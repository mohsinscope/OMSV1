// Infrastructure/Configurations/DocumentConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMSV1.Domain.Entities.Documents;

namespace OMSV1.Infrastructure.Configurations
{
    public class DocumentConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            // PK & index
            builder.HasKey(d => d.Id);
            builder.HasIndex(d => d.DocumentNumber).IsUnique();

            // Scalar props
            builder.Property(d => d.DocumentNumber).IsRequired().HasMaxLength(100);
            builder.Property(d => d.Title).IsRequired().HasMaxLength(200);
            builder.Property(d => d.DocumentType).IsRequired();
            builder.Property(d => d.ResponseType).IsRequired();
            builder.Property(d => d.Subject).HasMaxLength(500);
            builder.Property(d => d.IsRequiresReply).IsRequired();
            builder.Property(d => d.IsUrgent).IsRequired().HasDefaultValue(false);
            builder.Property(d => d.IsImportant).IsRequired().HasDefaultValue(false);
            builder.Property(d => d.IsNeeded).IsRequired().HasDefaultValue(false);
            builder.Property(d => d.DocumentDate).IsRequired();
            builder.Property(d => d.Notes).HasMaxLength(1000);
            builder.Property(d => d.IsReplied).IsRequired().HasDefaultValue(false);
            builder.Property(d => d.IsAudited).IsRequired().HasDefaultValue(false);

            // FKs
            builder.HasOne(d => d.Project)
                   .WithMany()
                   .HasForeignKey(d => d.ProjectId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Ministry)
                   .WithMany(m => m.Documents)
                   .HasForeignKey(d => d.MinistryId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(d => d.ParentDocument)
                   .WithMany(d => d.ChildDocuments)
                   .HasForeignKey(d => d.ParentDocumentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Party)
                   .WithMany()
                   .HasForeignKey(d => d.PartyId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Profile)
                   .WithMany()
                   .HasForeignKey(d => d.ProfileId)
                   .OnDelete(DeleteBehavior.Restrict);

            // CC many-to-many via DocumentCcLink
            builder.HasMany(d => d.CcLinks)
                   .WithOne(l => l.Document)
                   .HasForeignKey(l => l.DocumentId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(d => d.CCs)
                   .WithMany()
                   .UsingEntity<DocumentCcLink>(
                       j => j.HasOne(l => l.DocumentCc)
                             .WithMany(cc => cc.DocumentLinks)
                             .HasForeignKey(l => l.DocumentCcId),
                       j => j.HasOne(l => l.Document)
                             .WithMany(d => d.CcLinks)
                             .HasForeignKey(l => l.DocumentId),
                       j =>
                       {
                           j.ToTable("DocumentCcLinks");
                           j.HasKey(l => new { l.DocumentId, l.DocumentCcId });
                       });

            // Tags via explicit DocumentTagLink
            builder.HasMany(d => d.TagLinks)
                   .WithOne(l => l.Document)
                   .HasForeignKey(l => l.DocumentId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Documents");
        }
    }
}