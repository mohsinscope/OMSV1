using MediatR;
using OMSV1.Application.Dtos.Documents;
using OMSV1.Application.Dtos.GeneralDirectorates;
using OMSV1.Application.Helpers;

namespace OMSV1.Application.Queries.GeneralDirectories
{
    public class GetAllGeneralDirectoratesQuery : IRequest<PagedList<GeneralDirectorateDto>>
    {
        public PaginationParams PaginationParams { get; }

        public GetAllGeneralDirectoratesQuery(PaginationParams paginationParams)
        {
            PaginationParams = paginationParams;
        }
    }
}
