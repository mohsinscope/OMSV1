using System;
using AutoMapper;
using OMSV1.Application.Commands.Governorates;
using OMSV1.Application.Commands.Offices;
using OMSV1.Application.Dtos;
using OMSV1.Application.Dtos.DamagedDevices;
using OMSV1.Application.Dtos.Governorates;
using OMSV1.Application.Dtos.Offices;
using OMSV1.Domain.Entities.DamagedDevices;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.Entities.Offices;
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
           // Governorate mappings
            CreateMap<Governorate, GovernorateDto>();
            CreateMap<CreateGovernorateDto, Governorate>();
            CreateMap<AddGovernorateCommand, Governorate>();
            CreateMap<UpdateGovernorateCommand, Governorate>();
            // Other mappings

       CreateMap<Governorate, GovernorateWithOfficesDto>()
            .ForMember(dest => dest.Offices, opt => opt.MapFrom(src => src.Offices));
        CreateMap<Office, OfficeDto>();




        CreateMap<DamagedDevice, DamagedDeviceDto>()
            .ForMember(dest => dest.DeviceTypeName, 
                opt => opt.MapFrom(src => src.DeviceType.Name))
            .ForMember(dest => dest.GovernorateName, 
                opt => opt.MapFrom(src => src.Governorate.Name));
            
    }
    }

}