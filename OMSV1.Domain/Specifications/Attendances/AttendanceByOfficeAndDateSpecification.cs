using OMSV1.Domain.Entities.Attendances;

namespace OMSV1.Domain.Specifications.Attendances
{
    public class AttendanceByOfficeAndDateSpecification : BaseSpecification<Attendance>
    {
        public AttendanceByOfficeAndDateSpecification(Guid officeId, DateTime startDate, DateTime endDate)
            : base(x => x.OfficeId == officeId && x.Date >= startDate && x.Date < endDate)
        {
            // Optionally include relationships, for example:
            AddInclude(x => x.Office);
            
            // Apply ordering if needed (for instance, descending by date)
            ApplyOrderByDescending(x => x.Date);
        }
    }
}
