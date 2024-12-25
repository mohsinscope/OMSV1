using MediatR;
using OMSV1.Application.Dtos;
using OMSV1.Application.Helpers;  // Assuming PaginationParams and PagedList are in the Helpers namespace

namespace OMSV1.Application.Queries.DamagedPassports
{
    public class GetAllDamagedPassportsQuery : IRequest<PagedList<DamagedPassportAllDto>>
    {
        public PaginationParams PaginationParams { get; }

        public GetAllDamagedPassportsQuery(PaginationParams paginationParams)
        {
            PaginationParams = paginationParams;
        }
    }
}
