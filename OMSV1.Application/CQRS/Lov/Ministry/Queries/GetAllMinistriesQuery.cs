using MediatR;
using OMSV1.Application.Dtos.Documents;
using OMSV1.Application.Helpers;

namespace OMSV1.Application.Queries.Ministries
{
    public class GetAllMinistriesQuery : IRequest<PagedList<MinistryDto>>
    {
        public PaginationParams PaginationParams { get; }

        public GetAllMinistriesQuery(PaginationParams paginationParams)
        {
            PaginationParams = paginationParams;
        }
    }
}
