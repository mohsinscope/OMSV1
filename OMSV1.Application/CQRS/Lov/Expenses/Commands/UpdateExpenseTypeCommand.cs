using MediatR;

namespace OMSV1.Application.Commands.Expenses;

public class UpdateExpenseTypeCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public UpdateExpenseTypeCommand(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}