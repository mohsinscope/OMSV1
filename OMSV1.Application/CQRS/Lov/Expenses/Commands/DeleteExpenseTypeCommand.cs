using MediatR;

namespace OMSV1.Application.Commands.Expenses;

public class DeleteExpenseTypeCommand : IRequest<bool>
{
    public Guid Id { get; set; }

    public DeleteExpenseTypeCommand(Guid id)
    {
        Id = id;
    }
}