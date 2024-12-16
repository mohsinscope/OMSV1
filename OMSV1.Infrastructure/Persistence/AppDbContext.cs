using System;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OMSV1.Domain.Entities.Attachments;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Domain.Entities.DamagedPassport;
using OMSV1.Domain.Entities.Expenses;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.Entities.Profiles;
using OMSV1.Infrastructure.Identity;

namespace OMSV1.Infrastructure.Persistence;


public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser, AppRole, int,
    IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>,
    IdentityUserToken<int>>(options)
{
        public DbSet<DamagedDevice> DamagedDevices { get; set; }
        public   DbSet<DamagedPassport> DamagedPassports { get; set; }
        public   DbSet<DamagedType> DamagedTypes { get; set; }
        public   DbSet<DamagedDeviceType> DamagedDeviceTypes { get; set; }
        public   DbSet<DeviceType> DeviceTypes { get; set; }
        public  DbSet<AttachmentCU> AttachmentCUs { get; set; }
        
        public  DbSet<Office> Offices { get; set; }
        public  DbSet<Governorate> Governorates { get; set; }
        public  DbSet<Profile> Profiles { get; set; }
        public DbSet<OMSV1.Domain.Entities.Expenses.Action> Actions { get; set; }
        public DbSet<DailyExpenses> DailyExpenses { get; set; }
        public DbSet<ExpenseType> ExpenseTypes { get; set; }
        public DbSet<MonthlyExpenses> MonthlyExpenses { get; set; }





        public  DbSet<ApplicationUser> ApplicationUser { get; set; } 


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
