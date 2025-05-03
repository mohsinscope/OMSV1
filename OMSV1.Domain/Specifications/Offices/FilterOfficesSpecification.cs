using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.SeedWork;
using System;

namespace OMSV1.Domain.Specifications.Offices
{
    public class FilterOfficesSpecification : BaseSpecification<Office>
    {
        public FilterOfficesSpecification(
            Guid? governorateId,
            string? name,
            bool? isEmbassy,
            bool? isTwoShifts,
            int? code)
            : base(o =>
                (!governorateId.HasValue || o.GovernorateId == governorateId.Value) &&
                (string.IsNullOrEmpty(name) || o.Name.Contains(name)) &&
                (!isEmbassy.HasValue || o.IsEmbassy == isEmbassy) &&
                (!isTwoShifts.HasValue || o.IsTwoShifts == isTwoShifts) &&
                (!code.HasValue || o.Code == code.Value)
            )
        {
            // Include related Governorate if needed.
            AddInclude(o => o.Governorate);
            // Order the results by Name.
            ApplyOrderBy(o => o.Code);
        }
    }
}
