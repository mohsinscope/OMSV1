using MediatR;
using OMSV1.Application.DTOs.Expenses;
using System;

namespace OMSV1.Application.Queries.Expenses
{
public class GetStatisticsForLastTwoMonthsQuery : IRequest<ExpensesStatisticsDto>
{
}


}
