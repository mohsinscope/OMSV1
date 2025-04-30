using MediatR;

namespace OMSV1.Application.Commands.Expenses;

public class CreateMonthlyExpensesCommand : IRequest<Guid>
{
     public decimal TotalAmount { get; set; }
    public int Status { get; set; }

    public Guid OfficeId { get; set; }
    public Guid GovernorateId { get; set; }
    public Guid ProfileId { get; set; }

    public DateTime DateCreated { get; set; }
    public CreateMonthlyExpensesCommand(
        decimal totalAmount,
        int status,
        Guid officeId,
        Guid governorateId,
        Guid profileId,
        DateTime dateCreated
        )
    {
        TotalAmount = totalAmount;
        Status = status;
        OfficeId = officeId;
        GovernorateId = governorateId;
        ProfileId = profileId;
        DateCreated = dateCreated;
    }
}
