using System;

namespace OMSV1.Application.Dtos.LectureTypes
{
   public class LectureTypeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public LectureTypeDto(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
