using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMSV1.Domain.Entities.Sections;

namespace OMSV1.Infrastructure.Configurations
{

    public class SectionConfiguration : IEntityTypeConfiguration<Section>
    {
        public void Configure(EntityTypeBuilder<Section> builder)
        {
            builder.ToTable("Sections");
            builder.HasKey(sec => sec.Id);

            builder.Property(sec => sec.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasOne(sec => sec.Department)
                   .WithMany(dep => dep.Sections)
                   .HasForeignKey(sec => sec.DepartmentId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
