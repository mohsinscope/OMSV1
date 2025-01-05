using System;
using System.Collections.Generic;
using OMSV1.Application.Dtos.LectureTypes;

namespace OMSV1.Application.Dtos.Companies
{
    public class CompanyDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<LectureTypeDto> LectureTypes { get; set; } // List of LectureTypes for this company

        // Constructor to initialize the DTO
        public CompanyDto(Guid id, string name, List<LectureTypeDto> lectureTypes)
        {
            Id = id;
            Name = name;
            LectureTypes = lectureTypes;
        }
    }
}
