using System;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using OMSV1.Domain.Entities.Attachments;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Domain.Entities.DamagedPassport;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.Entities.Offices;

namespace OMSV1.Infrastructure.Persistence;


 public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public required DbSet<DamagedDevice> DamagedDevices { get; set; }
        public required DbSet<DamagedPassport> DamagedPassports { get; set; }
        public required DbSet<DamagedType> DamagedTypes { get; set; }
        public required DbSet<DamagedDeviceType> DamagedDeviceTypes { get; set; }
        public required DbSet<DeviceType> DeviceTypes { get; set; }
        // public required DbSet<AttachmentCU> AttachmentCU { get; set; }
        public required DbSet<Office> Offices { get; set; }
        public required DbSet<Governorate> Governorates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);

            // Fluent API configuration
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
