// Infrastructure/Configurations/DocumentConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.Entities.Sections;

namespace OMSV1.Infrastructure.Configurations
{
    public class DocumentConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            // Table
            builder.ToTable("Documents");

            // Primary Key
            builder.HasKey(d => d.Id);
            builder.HasIndex(d => d.DocumentNumber).IsUnique();

            // Scalar properties
            builder.Property(d => d.DocumentNumber)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(d => d.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(d => d.DocumentType)
                   .IsRequired();

            builder.Property(d => d.ResponseType)
                   .IsRequired();

            builder.Property(d => d.Subject)
                   .HasMaxLength(500);

            builder.Property(d => d.IsRequiresReply)
                   .IsRequired();

            builder.Property(d => d.IsUrgent)
                   .IsRequired()
                   .HasDefaultValue(false);

            builder.Property(d => d.IsImportant)
                   .IsRequired()
                   .HasDefaultValue(false);

            builder.Property(d => d.IsNeeded)
                   .IsRequired()
                   .HasDefaultValue(false);

            builder.Property(d => d.DocumentDate)
                   .IsRequired();

            builder.Property(d => d.Notes)
                   .HasMaxLength(1000);

            builder.Property(d => d.IsReplied)
                   .IsRequired()
                   .HasDefaultValue(false);

            builder.Property(d => d.IsAudited)
                   .IsRequired()
                   .HasDefaultValue(false);

            // Relationships
            // Project
       builder.HasOne(d => d.Project)
                   .WithMany()
                   .HasForeignKey(d => d.ProjectId)
                   .OnDelete(DeleteBehavior.Restrict);

        // Ministry           
       builder.HasOne(d => d.Ministry)
                   .WithMany()
                   .HasForeignKey(d => d.MinistryId)
                   .OnDelete(DeleteBehavior.Restrict);

       // GeneralDirectorate           
       builder.HasOne(d => d.GeneralDirectorate)
                   .WithMany()
                   .HasForeignKey(d => d.GeneralDirectorateId)
                   .OnDelete(DeleteBehavior.Restrict);

       // Directorate           
       builder.HasOne(d => d.Directorate)
                   .WithMany()
                   .HasForeignKey(d => d.DirectorateId)
                   .OnDelete(DeleteBehavior.Restrict);
                   
       // Department           
       builder.HasOne(d => d.Department)
                   .WithMany()
                   .HasForeignKey(d => d.DepartmentId)
                   .OnDelete(DeleteBehavior.Restrict);

       // Section           
       builder.HasOne(d => d.Section)
                   .WithMany()
                   .HasForeignKey(d => d.SectionId)
                   .OnDelete(DeleteBehavior.Restrict);
                   
                   




            // Profile
            builder.HasOne(d => d.Profile)
                   .WithMany()
                   .HasForeignKey(d => d.ProfileId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Parent/Child
            builder.HasOne(d => d.ParentDocument)
                   .WithMany(d => d.ChildDocuments)
                   .HasForeignKey(d => d.ParentDocumentId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Section (leaf)
       //      builder.HasOne(d => d.Section)
       //             .WithMany()   // Sections don't know about Documents directly
       //             .HasForeignKey(d => d.SectionId)
       //             .OnDelete(DeleteBehavior.SetNull);

       builder.HasOne(d => d.PrivateParty)
       .WithMany(pp => pp.Documents)
       .HasForeignKey(d => d.PrivatePartyId)
       .OnDelete(DeleteBehavior.SetNull);


            // CC links
            builder.HasMany(d => d.CcLinks)
                   .WithOne(link => link.Document)
                   .HasForeignKey(link => link.DocumentId)
                   .OnDelete(DeleteBehavior.SetNull);

            // Tag links
            builder.HasMany(d => d.TagLinks)
                   .WithOne(link => link.Document)
                   .HasForeignKey(link => link.DocumentId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
