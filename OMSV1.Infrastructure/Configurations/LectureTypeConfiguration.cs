using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMSV1.Domain.Entities.Companies;
using OMSV1.Domain.Entities.Lectures;

namespace OMSV1.Infrastructure.Configurations
{
    public class LectureTypeConfiguration : IEntityTypeConfiguration<LectureType>
    {
        public void Configure(EntityTypeBuilder<LectureType> builder)
        {
            builder.HasKey(ddt => ddt.Id);

            builder.Property(ddt => ddt.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Add configuration for many-to-many relationship
            builder.HasMany(lt => lt.LectureLectureTypes)
                .WithOne(llt => llt.LectureType)
                .HasForeignKey(llt => llt.LectureTypeId);

            // Table Mapping
            builder.ToTable("LectureType");
        }
    }
}