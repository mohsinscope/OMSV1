// LectureLectureTypeConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMSV1.Domain.Entities.Lectures;

namespace OMSV1.Infrastructure.Configurations
{
    public class LectureLectureTypeConfiguration : IEntityTypeConfiguration<LectureLectureType>
    {
        public void Configure(EntityTypeBuilder<LectureLectureType> builder)
        {
            builder.HasKey(llt => new { llt.LectureId, llt.LectureTypeId });

            builder.HasOne(llt => llt.Lecture)
                .WithMany(l => l.LectureLectureTypes)
                .HasForeignKey(llt => llt.LectureId);

            builder.HasOne(llt => llt.LectureType)
                .WithMany()
                .HasForeignKey(llt => llt.LectureTypeId);

            builder.ToTable("LectureLectureTypes");
        }
    }
}