using System.Reflection.Metadata;
using iTextSharp.text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OMSV1.Domain.Entities.Attachments;
using OMSV1.Domain.Entities.Attendances;
using OMSV1.Domain.Entities.Companies;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Domain.Entities.DamagedPassport;
using OMSV1.Domain.Entities.Directorates;
using OMSV1.Domain.Entities.Expenses;
using OMSV1.Domain.Entities.GeneralDirectorates;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.Entities.Lectures;
using OMSV1.Domain.Entities.Ministries;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.Entities.Profiles;
using OMSV1.Domain.Entities.Reports;
using OMSV1.Domain.Entities.Sections;
using OMSV1.Infrastructure.Identity;

namespace OMSV1.Infrastructure.Persistence;


public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser, AppRole, Guid,
    IdentityUserClaim<Guid>, AppUserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>,
    IdentityUserToken<Guid>>(options)
{
        public DbSet<DamagedDevice> DamagedDevices { get; set; }
        public DbSet<Lecture> Lectures { get; set; }

        public   DbSet<DamagedPassport> DamagedPassports { get; set; }
        public   DbSet<DamagedType> DamagedTypes { get; set; }
        public   DbSet<DamagedDeviceType> DamagedDeviceTypes { get; set; }
        public   DbSet<DeviceType> DeviceTypes { get; set; }
        public  DbSet<AttachmentCU> AttachmentCUs { get; set; }
        
        public  DbSet<Office> Offices { get; set; }
        public  DbSet<Governorate> Governorates { get; set; }
        public  DbSet<Profile> Profiles { get; set; }
        public DbSet<Attendance> Attendances { get; set; }

        public DbSet<OMSV1.Domain.Entities.Expenses.Action> Actions { get; set; }
        public DbSet<DailyExpenses> DailyExpenses { get; set; }
        public DbSet<ExpenseType> ExpenseTypes { get; set; }
        public DbSet<MonthlyExpenses> MonthlyExpenses { get; set; }





        public  DbSet<ApplicationUser> ApplicationUser { get; set; } 
        //Permissions
        public DbSet<AppRolePermission> RolePermissions { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<LectureType> LectureType { get; set; }
        public DbSet<Threshold> Thresholds {get;set;}
        public DbSet<EmailReport> EmailReports {get;set;}
        public DbSet<ReportType> ReportTypes {get;set;}
        public DbSet<OMSV1.Domain.Entities.Documents.Document> Documents {get;set;}
        public DbSet<OMSV1.Domain.Entities.DocumentHistories.DocumentHistory> DocumentHistories {get;set;}
        public DbSet<OMSV1.Domain.Entities.Projects.Project> Projects {get;set;}
        public DbSet<Ministry> Ministries { get; set; }
        public DbSet<GeneralDirectorate> GeneralDirectorates { get; set; }
        public DbSet<Directorate> Directorates { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<OMSV1.Domain.Entities.Sections.Section> Sections { get; set; }
                public DbSet<OMSV1.Domain.Entities.Attachments.DocumentAttachment> DocumentAttachments { get; set; }






        public DbSet<OMSV1.Domain.Entities.Documents.DocumentCC> DocumentCCs {get;set;}
        public DbSet<OMSV1.Domain.Entities.Documents.Tag> Tags {get;set;}














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

        modelBuilder.Entity<AppRolePermission>(entity =>
            {
            entity.HasKey(rp => rp.Id);
            entity.HasOne(rp => rp.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(rp => rp.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
             });
             
        modelBuilder.Entity<UserPermission>(entity =>
        {
        // Configure the primary key
        entity.HasKey(up => up.Id);

        // Ensure the Id column generates UUIDs by default
        entity.Property(up => up.Id)
            .HasDefaultValueSql("gen_random_uuid()"); // PostgreSQL's UUID generator

        // Configure the foreign key relationship with the User entity
        entity.HasOne(up => up.User)
            .WithMany()
            .HasForeignKey(up => up.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure the Permission property
        entity.Property(up => up.Permission)
            .IsRequired()
            .HasMaxLength(255);
        });

            

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
