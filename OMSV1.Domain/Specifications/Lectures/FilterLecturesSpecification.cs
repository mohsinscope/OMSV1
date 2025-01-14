using System;
using System.Collections.Generic;
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
                Guid? companyId = null,
                List<Guid>? lectureTypeIds = null) // Updated to List<Guid>?
                : base(x =>
                    (string.IsNullOrEmpty(title) || x.Title.Contains(title)) &&
                    (!startDate.HasValue || x.Date >= startDate.Value) &&
                    (!endDate.HasValue || x.Date <= endDate.Value) &&
                    (!officeId.HasValue || x.OfficeId == officeId.Value) &&
                    (!governorateId.HasValue || x.GovernorateId == governorateId.Value) &&
                    (!profileId.HasValue || x.ProfileId == profileId.Value) &&
                    (!companyId.HasValue || x.CompanyId == companyId.Value) &&
                    (lectureTypeIds == null || lectureTypeIds.Count == 0 || x.LectureLectureTypes.Any(lt => lectureTypeIds.Contains(lt.LectureTypeId))))
            {
                AddInclude(x => x.Governorate);
                AddInclude(x => x.Office);
                AddInclude(x => x.Profile);
                AddInclude(x => x.Company);
                AddInclude(x => x.LectureLectureTypes);
                AddInclude($"{nameof(Lecture.LectureLectureTypes)}.{nameof(LectureLectureType.LectureType)}");

                ApplyOrderByDescending(x => x.Date);
            }
        }

}
