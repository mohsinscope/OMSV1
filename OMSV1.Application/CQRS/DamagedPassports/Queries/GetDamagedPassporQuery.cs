using MediatR;
using OMSV1.Application.Dtos;
using OMSV1.Application.Helpers;

namespace OMSV1.Application.CQRS.DamagedPassports.Queries;
    public class GetDamagedPassportQuery : IRequest<PagedList<DamagedPassportDto>>
        {
            public int? GovernorateId { get; set; }
            public string? PassportNumber { get; set; }

            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public int? DamagedTypeId { get; set; }
            public int? OfficeId { get; set; }
            public int? ProfileId { get; set; }
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 10;
            public PaginationParams PaginationParams { get;set; }

            public GetDamagedPassportQuery (PaginationParams paginationParams)
            {
                PaginationParams = paginationParams;
            }


        }