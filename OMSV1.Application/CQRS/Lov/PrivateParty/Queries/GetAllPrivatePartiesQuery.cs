// Application/Queries/Documents/GetAllPrivatePartiesQuery.cs
using MediatR;
using OMSV1.Application.Dtos.Documents;
using OMSV1.Application.Helpers;

namespace OMSV1.Application.Queries.Documents
{
    public class GetAllPrivatePartiesQuery : IRequest<PagedList<PrivatePartyDto>>
    {
        public PaginationParams PaginationParams { get; }

        public GetAllPrivatePartiesQuery(PaginationParams paginationParams)
        {
            PaginationParams = paginationParams;
        }
    }
}
