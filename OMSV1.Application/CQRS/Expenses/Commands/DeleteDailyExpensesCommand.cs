using MediatR;

namespace OMSV1.Application.Commands.Expenses;

public class DeleteDailyExpensesCommand : IRequest<bool>
{
    public Guid Id { get; set; }

    public DeleteDailyExpensesCommand(Guid id)
    {
        Id = id;
    }
}
