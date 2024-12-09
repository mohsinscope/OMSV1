using System;
using AutoMapper;
using OMSV1.Application.Dtos;
using OMSV1.Infrastructure.Identity;

namespace OMSV1.Application.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {

        CreateMap<RegisterDto,ApplicationUser>();
        CreateMap<string, DateOnly>().ConvertUsing(s => DateOnly.Parse(s));

        CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
        CreateMap<DateTime?, DateTime?>().ConvertUsing(d => d.HasValue 
            ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : null);
    }

}