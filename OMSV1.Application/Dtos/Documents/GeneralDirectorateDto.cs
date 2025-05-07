// Application/Dtos/GeneralDirectorates/GeneralDirectorateDto.cs
using System;
using OMSV1.Domain.Entities.Ministries;

namespace OMSV1.Application.Dtos.GeneralDirectorates
{
    public class GeneralDirectorateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid MinistryId { get; set; }
        public string MinistryName      { get; set; } = string.Empty;

        public DateTime DateCreated { get; set; }
    }
}
