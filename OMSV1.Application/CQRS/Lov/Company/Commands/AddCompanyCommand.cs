using MediatR;
using System;

namespace OMSV1.Application.Commands.Companies
{
    public class AddCompanyCommand : IRequest<Guid> // Returns the ID of the newly created Company
    {
        public string Name { get; set; }

        public AddCompanyCommand(string name)
        {
            Name = name;
        }
    }
}
