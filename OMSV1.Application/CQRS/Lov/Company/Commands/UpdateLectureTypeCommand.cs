using MediatR;
using System;

namespace OMSV1.Application.Commands.LectureTypes
{
    public class UpdateLectureTypeCommand : IRequest<bool>
    {
        public Guid LectureTypeId { get; set; }
        public string Name { get; set; }

        public UpdateLectureTypeCommand(Guid lectureTypeId, string name)
        {
            LectureTypeId = lectureTypeId;
            Name = name;
        }
    }
}
