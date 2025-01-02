using System;
using OMSV1.Domain.Entities.Offices;

namespace OMSV1.Domain.Specifications.Offices;

public class FilterOfficeSpecification : BaseSpecification<Office>
{
    public FilterOfficeSpecification(Guid? governorateId = null, int? officeId = null)
        : base(x =>
            (!governorateId.HasValue || x.GovernorateId == governorateId.Value) &&
            (!officeId.HasValue || x.Code == officeId.Value))
    {
        // Include related Governorate entity
        AddInclude(x => x.Governorate);

        // Apply ordering if required
    }
}
