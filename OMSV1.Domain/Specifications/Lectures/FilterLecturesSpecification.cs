using System;
using OMSV1.Domain.Entities.Lectures;

namespace OMSV1.Domain.Specifications.Lectures
{
    public class FilterLecturesSpecification : BaseSpecification<Lecture>
    {
        public FilterLecturesSpecification(
            string? title = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            Guid? officeId = null,
            Guid? governorateId = null,
            Guid? profileId = null,
            Guid? companyId = null, // New parameter for CompanyId
            Guid? lectureTypeId = null) // New parameter for LectureTypeId
            : base(x =>
                (string.IsNullOrEmpty(title) || x.Title.Contains(title)) &&
                (!startDate.HasValue || x.Date >= startDate.Value) &&
                (!endDate.HasValue || x.Date <= endDate.Value) &&
                (!officeId.HasValue || x.OfficeId == officeId.Value) &&
                (!governorateId.HasValue || x.GovernorateId == governorateId.Value) &&
                (!profileId.HasValue || x.ProfileId == profileId.Value) &&
                (!companyId.HasValue || x.CompanyId == companyId.Value) )// Filter by CompanyId
        {
            // Include related entities for eager loading
            AddInclude(x => x.Governorate);
            AddInclude(x => x.Office);
            AddInclude(x => x.Profile);
            AddInclude(x => x.Company); // Include Company

            // Apply ordering by date (descending)
            ApplyOrderByDescending(x => x.Date);
        }
    }
}
