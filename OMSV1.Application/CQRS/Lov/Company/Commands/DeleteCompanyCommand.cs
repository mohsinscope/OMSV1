using MediatR;

namespace OMSV1.Application.Commands.Companies
{
    public class DeleteCompanyCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public DeleteCompanyCommand(Guid id)
        {
            Id = id;
        }
    }
}
