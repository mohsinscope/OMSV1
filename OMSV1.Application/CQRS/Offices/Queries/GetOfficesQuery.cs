using MediatR;
using OMSV1.Application.Dtos.Offices;
using OMSV1.Application.Helpers;

namespace OMSV1.Application.CQRS.Offices.Queries
{
    public class GetOfficesQuery : IRequest<PagedList<OfficeDto>>
    {
        public Guid? GovernorateId { get; set; }
        public string? Name { get; set; }
        public int? Code { get; set; }
        public bool? IsEmbassy { get; set; }
        public PaginationParams PaginationParams { get; set; }

        public GetOfficesQuery(PaginationParams paginationParams)
        {
            PaginationParams = paginationParams;
        }
    }
}
