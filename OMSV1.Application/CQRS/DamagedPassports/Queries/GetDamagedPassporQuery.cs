using MediatR;
using OMSV1.Application.Dtos;
using OMSV1.Application.Helpers;

namespace OMSV1.Application.CQRS.DamagedPassports.Queries;
    public class GetDamagedPassportQuery : IRequest<PagedList<DamagedPassportDto>>
        {
            public Guid? GovernorateId { get; set; }
            public string? PassportNumber { get; set; }

            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public Guid? DamagedTypeId { get; set; }
            public Guid? OfficeId { get; set; }
            public Guid? ProfileId { get; set; }
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 10;
            public PaginationParams PaginationParams { get;set; }

            public GetDamagedPassportQuery (PaginationParams paginationParams)
            {
                PaginationParams = paginationParams;
            }


        }