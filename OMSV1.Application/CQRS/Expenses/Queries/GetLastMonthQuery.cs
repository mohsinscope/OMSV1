using MediatR;
using OMSV1.Application.DTOs.Expenses;
using OMSV1.Domain.Enums;

namespace OMSV1.Application.Queries.Expenses;

public class GetLastMonthQuery : IRequest<List<MonthlyExpensesDto>>  // Changed return type to List
{
    public Guid? OfficeId { get; set; }
    public Status? Status { get; set; }

    // Parameterless constructor for deserialization
    public GetLastMonthQuery() { }

    // Parameterized constructor for manual instantiation
    public GetLastMonthQuery(Guid? officeId, Status status)
    {
        OfficeId = officeId;
        Status = status;
    }
}