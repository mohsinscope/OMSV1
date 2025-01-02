using MediatR;
using OMSV1.Application.Dtos.Offices;

namespace OMSV1.Application.Queries.Offices
{
    public class GetOfficeByIdQuery : IRequest<OfficeDto>
    {
        public Guid OfficeId { get; }

        public GetOfficeByIdQuery(Guid officeId)
        {
            OfficeId = officeId;
        }
    }
}
