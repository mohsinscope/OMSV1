using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMSV1.Domain.Entities.Sections;

namespace OMSV1.Infrastructure.Configurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Domain.Entities.Sections.Department>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Sections.Department> builder)
        {
            builder.ToTable("Departments");
            builder.HasKey(dep => dep.Id);

            builder.Property(dep => dep.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasOne(dep => dep.Directorate)
                   .WithMany(d => d.Departments)
                   .HasForeignKey(dep => dep.DirectorateId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(dep => dep.Sections)
                   .WithOne(sec => sec.Department)
                   .HasForeignKey(sec => sec.DepartmentId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
