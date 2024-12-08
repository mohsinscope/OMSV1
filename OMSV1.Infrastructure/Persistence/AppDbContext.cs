using System;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OMSV1.Domain.Entities.Attachments;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Domain.Entities.DamagedPassport;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Infrastructure.Identity;

namespace OMSV1.Infrastructure.Persistence;


public class AppDbContext : IdentityDbContext<ApplicationUser, AppRole, int,
    IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>,
    IdentityUserToken<int>>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options)
    {
    }
        public required DbSet<DamagedDevice> DamagedDevices { get; set; }
        public required  DbSet<DamagedPassport> DamagedPassports { get; set; }
        public required  DbSet<DamagedType> DamagedTypes { get; set; }
        public required  DbSet<DamagedDeviceType> DamagedDeviceTypes { get; set; }
        public required  DbSet<DeviceType> DeviceTypes { get; set; }
        public required DbSet<AttachmentCU> AttachmentCUs { get; set; }
        public required DbSet<Office> Offices { get; set; }
        public required DbSet<Governorate> Governorates { get; set; }

        public required DbSet<ApplicationUser> ApplicationUser { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>()
            .HasMany(ur => ur.UserRoles)
            .WithOne(u => u.User)
            .HasForeignKey(ur => ur.UserId)
            .IsRequired();

        modelBuilder.Entity<AppRole>()
            .HasMany(ur => ur.UserRoles) 
            .WithOne(u => u.Role)
            .HasForeignKey(ur => ur.RoleId)
            .IsRequired();

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
