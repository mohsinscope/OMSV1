using MediatR;

namespace OMSV1.Application.Commands.Expenses;

public class CreateExpenseTypeCommand : IRequest<Guid>
{
    public string Name { get; set; } = string.Empty;

    public CreateExpenseTypeCommand(string name)
    {
        Name = name;
    }
}
