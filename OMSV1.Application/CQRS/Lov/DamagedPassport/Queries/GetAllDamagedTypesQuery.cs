using MediatR;
using OMSV1.Application.Dtos.LOV;
using OMSV1.Application.Helpers; // For PaginationParams

namespace OMSV1.Application.CQRS.Lov.DamagedPassport
{
    // Note: The return type has been changed from List<T> to PagedList<T>
    public class GetAllDamagedTypesQuery : IRequest<PagedList<DamagedTypeDto>>
    {
        public PaginationParams PaginationParams { get; }

        public GetAllDamagedTypesQuery(PaginationParams paginationParams)
        {
            PaginationParams = paginationParams;
        }
    }
}
