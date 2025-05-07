using System.Reflection.Metadata;
using AutoMapper;
using OMSV1.Application.Commands.Attendances;
using OMSV1.Application.Commands.DamagedDevices;
using OMSV1.Application.Commands.DamagedPassports;
using OMSV1.Application.Commands.Governorates;
using OMSV1.Application.Commands.Lectures;
using OMSV1.Application.Commands.Offices;
using OMSV1.Application.Dtos;
using OMSV1.Application.Dtos.Attendances;
using OMSV1.Application.Dtos.Companies;
using OMSV1.Application.Dtos.DamagedDevices;
using OMSV1.Application.Dtos.Departments;
using OMSV1.Application.Dtos.Directorates;
using OMSV1.Application.Dtos.Documents;
using OMSV1.Application.Dtos.Expenses;
using OMSV1.Application.Dtos.GeneralDirectorates;
using OMSV1.Application.Dtos.Governorates;
using OMSV1.Application.Dtos.Lectures;
using OMSV1.Application.Dtos.LectureTypes;
using OMSV1.Application.Dtos.LOV;
using OMSV1.Application.Dtos.Offices;
using OMSV1.Application.Dtos.Profiles;
using OMSV1.Application.Dtos.Projects;
using OMSV1.Application.Dtos.Sections;
using OMSV1.Application.DTOs.Expenses;
using OMSV1.Application.DTOs.Reports;
using OMSV1.Domain.Entities.Attendances;
using OMSV1.Domain.Entities.Companies;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Domain.Entities.DamagedPassport;
using OMSV1.Domain.Entities.Directorates;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.Entities.Expenses;
using OMSV1.Domain.Entities.GeneralDirectorates;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.Entities.Lectures;
using OMSV1.Domain.Entities.Ministries;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.Entities.Projects;
using OMSV1.Domain.Entities.Reports;
using OMSV1.Domain.Entities.Sections;
using OMSV1.Infrastructure.Identity;
namespace OMSV1.Application.Helpers{

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {

            CreateMap<RegisterDto,ApplicationUser>();
            CreateMap<string, DateOnly>().ConvertUsing(s => DateOnly.Parse(s));
            CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
            CreateMap<DateTime?, DateTime?>().ConvertUsing(d => d.HasValue 
            ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : null);
           // New mappings for Office
           CreateMap<Office, OfficeDto>();
           CreateMap<CreateOfficeDto, Office>();
           CreateMap<UpdateOfficeCommand, Office>();
           CreateMap<AddOfficeCommand, Office>(); 
           //Expenses Mappings
           CreateMap<ExpenseType,ExpenseTypeDto>();
            CreateMap<MonthlyExpenses, MonthlyExpensesDto>();
            CreateMap<MonthlyExpenses, MonthlyCleanDto>();
            CreateMap<DailyExpenses, DailyExpensesDto>();
            CreateMap<OMSV1.Domain.Entities.Expenses.Action,ActionDto>();
            //Documents
            // CreateMap<DocumentParty, DocumentPartyDto>();
            CreateMap<OMSV1.Domain.Entities.Documents.Document, DocumentDto>()
                .ForMember(dest => dest.SectionName,
        opt => opt.MapFrom(src => src.Section.Name))
    .ForMember(dest => dest.DepartmentName,
        opt => opt.MapFrom(src => src.Section.Department.Name))
    .ForMember(dest => dest.DirectorateName,
        opt => opt.MapFrom(src => src.Section.Department.Directorate.Name))
    .ForMember(dest => dest.GeneralDirectorateName,
        opt => opt.MapFrom(src => src.Section.Department.Directorate.GeneralDirectorate.Name))
    .ForMember(dest => dest.MinistryName,
        opt => opt.MapFrom(src => src.Section.Department.Directorate.GeneralDirectorate.Ministry.Name))
    // ...and similarly for all the ID and Name members...
    ;
            CreateMap<Project, ProjectDto>();
            CreateMap<DocumentCC, DocumentCCDto>();
            CreateMap<Ministry,MinistryDto>();
            CreateMap<GeneralDirectorate,GeneralDirectorateDto>();
            CreateMap<Directorate,DirectorateDto>();
            CreateMap<Department,DepartmentDto>();
            CreateMap<Section,SectionDto>();
            CreateMap<PrivateParty,PrivatePartyDto>();





            CreateMap<Tag,TagsDto>();
            CreateMap<DocumentCcLink, DocumentCCDto>();
            CreateMap<DocumentTagLink, TagsDto>();




           // Governorate mappings
            CreateMap<Governorate, GovernorateDto>();
            CreateMap<CreateGovernorateDto, Governorate>();
            CreateMap<AddGovernorateCommand, Governorate>();
            CreateMap<UpdateGovernorateCommand, Governorate>();
            CreateMap<Governorate, GovernorateWithOfficesDto>()
            .ForMember(dest => dest.Offices, opt => opt.MapFrom(src => src.Offices));
            //Mapping for adding a new damaged paspport
            CreateMap<AddDamagedPassportWithAttachmentCommand, DamagedPassport>();
            CreateMap<UpdateDamagedPassportCommand, DamagedPassport>();
            CreateMap<DamagedPassport, DamagedPassportDto>();
            CreateMap<DamagedPassport, DamagedPassportAllDto>();

            // Mapping for adding a new DamagedDevice
            CreateMap<AddDamagedDeviceCommand, DamagedDevice>();
            CreateMap<DamagedDevice, DamagedDeviceDto>();
            CreateMap<DamagedDevice, DamagedDeviceAllDto>();
            //Email
            CreateMap<ReportType, ReportTypeDto>();
            CreateMap<EmailReport, EmailReportDto>()
            .ForMember(dest => dest.ReportTypes, opt => opt.MapFrom(src => src.ReportTypes));
            CreateMap<DamagedType, DamagedTypeDto>();



            //Lecture
            CreateMap<AddLectureCommand,Lecture>();
             CreateMap<Lecture, LectureDto>()
            .ForMember(dest => dest.LectureTypeNames, opt => opt.MapFrom(src =>
                src.LectureLectureTypes.Select(llt => llt.LectureType.Name).ToList()));
            CreateMap<Lecture,LectureAllDto>();
            CreateMap <Company,CompanyDto>();
            CreateMap <LectureType,LectureTypeDto>();
            CreateMap <LectureType,LectureTypeAllDto>();


            // Profile Mapping
            CreateMap<Profile, ProfileDto>();
            CreateMap<OMSV1.Domain.Entities.Profiles.Profile,ProfileWithUserAndRolesDto>();
            CreateMap<OMSV1.Domain.Entities.Profiles.Profile,ProfileDto>();

            //Attendance Mapping
            CreateMap<Attendance, AttendanceDto>();
            CreateMap<Attendance, AttendanceAllDto>();
            CreateMap<CreateAttendanceCommand, Attendance>();



    




           
            
    }
    }

}