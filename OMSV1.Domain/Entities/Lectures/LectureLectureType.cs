using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.Lectures
{
    public class LectureLectureType(Guid lectureId, Guid lectureTypeId) : Entity  // Inherit from Entity
    {
        public Guid LectureId { get; private set; } = lectureId;
        public Guid LectureTypeId { get; private set; } = lectureTypeId;

        public Lecture? Lecture { get; private set; }
        public LectureType? LectureType { get; private set; }
    }
}
