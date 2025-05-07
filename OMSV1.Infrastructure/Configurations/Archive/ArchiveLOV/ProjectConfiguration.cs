// Infrastructure/Configurations/ProjectConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMSV1.Domain.Entities.Projects;

namespace OMSV1.Infrastructure.Configurations
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            // Primary Key
            builder.HasKey(p => p.Id);

            // Name
            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            // // Documents relationship
            // builder.HasMany(p => p.Documents)
            //        .WithOne(d => d.Project)
            //        .HasForeignKey(d => d.ProjectId)
            //        .OnDelete(DeleteBehavior.Restrict);

            // Table Mapping
            builder.ToTable("Projects");
        }
    }
}
