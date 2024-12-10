using MediatR;
using OMSV1.Domain.Entities.Offices;

namespace OMSV1.Application.Queries.Offices
{
    public class GetOfficeByIdQuery : IRequest<Office>
    {
        public int OfficeId { get; }

        public GetOfficeByIdQuery(int officeId)
        {
            OfficeId = officeId;
        }
    }
}
