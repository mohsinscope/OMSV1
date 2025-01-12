using MediatR;
using OMSV1.Application.DTOs.Expenses;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Enums;
using System;

namespace OMSV1.Application.Queries.Expenses
{
    public class GetFilteredMonthlyExpensesQuery : IRequest<PagedList<MonthlyExpensesDto>>
    {
        public Guid? OfficeId { get; set; }
        public Guid? GovernorateId { get; set; }
        public Guid? ProfileId { get; set; }
        public Status? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public PaginationParams PaginationParams { get; set; }

        public GetFilteredMonthlyExpensesQuery(
            Guid? officeId,
            Guid? governorateId,
            Guid? profileId,
            Status? status,
            DateTime? startDate,
            DateTime? endDate,
            PaginationParams paginationParams)
        {
            OfficeId = officeId;
            GovernorateId = governorateId;
            ProfileId = profileId;
            Status = status;
            StartDate = startDate;
            EndDate = endDate;
            PaginationParams = paginationParams;
        }
    }
}
