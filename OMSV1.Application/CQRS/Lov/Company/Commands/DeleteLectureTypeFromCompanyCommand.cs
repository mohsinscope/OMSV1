using MediatR;
using System;

namespace OMSV1.Application.Commands.LectureTypes
{
    public class DeleteLectureTypeCommand : IRequest<bool> // Returns a boolean indicating success or failure
    {
        public Guid LectureTypeId { get; set; }

        public DeleteLectureTypeCommand(Guid lectureTypeId)
        {
            LectureTypeId = lectureTypeId;
        }
    }
}
