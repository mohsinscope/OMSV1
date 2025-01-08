using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.Lectures
{
    public class LectureLectureType : Entity  // Inherit from Entity
    {
        public Guid LectureId { get; private set; }
        public Guid LectureTypeId { get; private set; }
        
        public Lecture Lecture { get; private set; }
        public LectureType LectureType { get; private set; }

        // Constructor
        public LectureLectureType(Guid lectureId, Guid lectureTypeId)
        {
            LectureId = lectureId;
            LectureTypeId = lectureTypeId;
        }
    }
}
