using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMSV1.Domain.Entities.Projects;
using OMSV1.Domain.Entities.Documents;

namespace OMSV1.Infrastructure.Configurations
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            // Primary Key (assuming the base Entity defines an Id property)
            builder.HasKey(p => p.Id);

            // Configure the Name property as required with a maximum length.
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200); // Adjust the max length as necessary.

            // Configure one-to-many relationship:
            // One project has many Documents.
            builder.HasMany(p => p.Documents)
                .WithOne(d => d.Project)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            // Table Mapping
            builder.ToTable("Projects");
        }
    }
}
