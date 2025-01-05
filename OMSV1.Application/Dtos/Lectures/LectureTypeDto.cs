using System;

namespace OMSV1.Application.Dtos.LectureTypes
{
    public class LectureTypeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        // Constructor to initialize the DTO
        public LectureTypeDto(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
