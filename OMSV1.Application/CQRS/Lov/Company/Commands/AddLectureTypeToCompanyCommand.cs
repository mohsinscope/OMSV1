using MediatR;
using System;

namespace OMSV1.Application.Commands.LectureTypes
{
    public class AddLectureTypeToCompanyCommand : IRequest<Guid> // Returns the ID of the newly created LectureType
    {
        public string Name { get; set; }
        public Guid CompanyId { get; set; }

        public AddLectureTypeToCompanyCommand(string name, Guid companyId)
        {
            Name = name;
            CompanyId = companyId;
        }
    }
}
