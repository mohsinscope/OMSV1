using System;
using OMSV1.Domain.Entities.Lectures;

namespace OMSV1.Domain.Specifications.Lectures;

public class FilterLecturesSpecification : BaseSpecification<Lecture>
{
    public FilterLecturesSpecification(
        string? title = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        Guid? officeId = null,
        Guid? governorateId = null,
        Guid? profileId = null)
        : base(x =>
            (string.IsNullOrEmpty(title) || x.Title.Contains(title)) &&
            (!startDate.HasValue || x.Date >= startDate.Value) &&
            (!endDate.HasValue || x.Date <= endDate.Value) &&
            (!officeId.HasValue || x.OfficeId == officeId.Value) &&
            (!governorateId.HasValue || x.GovernorateId == governorateId.Value) &&
            (!profileId.HasValue || x.ProfileId == profileId.Value))
    {
        AddInclude(x => x.Governorate);
        AddInclude(x => x.Office);
        AddInclude(x => x.Profile);
                   // Apply ordering
         ApplyOrderByDescending(x => x.Date);
    }
}
