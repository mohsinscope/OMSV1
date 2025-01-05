using MediatR;
using OMSV1.Application.Dtos.Companies;
using OMSV1.Application.Helpers;

namespace OMSV1.Application.Queries.Companies
{
    public class GetAllCompaniesQuery : IRequest<PagedList<CompanyDto>>
    {
        public PaginationParams PaginationParams { get; }

        public GetAllCompaniesQuery(PaginationParams paginationParams)
        {
            PaginationParams = paginationParams;
        }
    }
}
