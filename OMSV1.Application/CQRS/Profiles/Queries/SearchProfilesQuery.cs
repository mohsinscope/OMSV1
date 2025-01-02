using MediatR;
using OMSV1.Application.CQRS.Queries.Profiles;
using OMSV1.Application.Dtos;
using OMSV1.Application.Dtos.Profiles;
using OMSV1.Application.Helpers;

namespace OMSV1.Application.CQRS.Profiles.Queries;
    public class SearchProfilesQuery : IRequest<PagedList<ProfileWithUserAndRolesDto>>
        {
            public string? FullName { get; set; }
            public Guid? OfficeId { get; set; }
            public Guid? GovernorateId { get; set; }
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 10;
            public PaginationParams PaginationParams { get;set; }

            public SearchProfilesQuery (PaginationParams paginationParams)
            {
                PaginationParams = paginationParams;
            }


        }