// LectureConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMSV1.Domain.Entities.Lectures;

namespace OMSV1.Infrastructure.Configurations
{
    public class LectureConfiguration : IEntityTypeConfiguration<Lecture>
    {
        public void Configure(EntityTypeBuilder<Lecture> builder)
        {
            builder.HasKey(l => l.Id);

            builder.Property(l => l.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasIndex(dd => dd.Title)
                .IsUnique(false);

            builder.Property(l => l.Date)
                .IsRequired();
            
            builder.Property(a => a.Note)
                .HasMaxLength(500); 

            builder.Property(l => l.OfficeId)
                .IsRequired();

            builder.Property(l => l.GovernorateId)
                .IsRequired();

            builder.Property(l => l.ProfileId)
                .IsRequired();

            builder.Property(l => l.CompanyId)
                .IsRequired(false);

            builder.HasOne(l => l.Governorate)
                .WithMany()
                .HasForeignKey(l => l.GovernorateId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(l => l.Office)
                .WithMany()
                .HasForeignKey(l => l.OfficeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(l => l.Profile)
                .WithMany()
                .HasForeignKey(l => l.ProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(l => l.Company)
                .WithMany()
                .HasForeignKey(l => l.CompanyId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);

            builder.HasMany(l => l.LectureLectureTypes)
                .WithOne(llt => llt.Lecture)
                .HasForeignKey(llt => llt.LectureId);

            builder.ToTable("Lectures");
        }
    }
}