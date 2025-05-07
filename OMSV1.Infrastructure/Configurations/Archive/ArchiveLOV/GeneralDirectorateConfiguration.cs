// GeneralDirectorateConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMSV1.Domain.Entities.GeneralDirectorates;

    public class GeneralDirectorateConfiguration : IEntityTypeConfiguration<GeneralDirectorate>
    {
        public void Configure(EntityTypeBuilder<GeneralDirectorate> builder)
        {
            builder.ToTable("GeneralDirectorates");
            builder.HasKey(gd => gd.Id);

            builder.Property(gd => gd.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasOne(gd => gd.Ministry)
                   .WithMany(m => m.GeneralDirectorates)
                   .HasForeignKey(gd => gd.MinistryId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(gd => gd.Directorates)
                   .WithOne(d => d.GeneralDirectorate)
                   .HasForeignKey(d => d.GeneralDirectorateId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
