using MediatR;
using OMSV1.Application.Dtos.Offices;

namespace OMSV1.Application.Queries.Offices
{
    public class GetOfficeByIdQuery : IRequest<OfficeDto>
    {
        public int OfficeId { get; }

        public GetOfficeByIdQuery(int officeId)
        {
            OfficeId = officeId;
        }
    }
}
