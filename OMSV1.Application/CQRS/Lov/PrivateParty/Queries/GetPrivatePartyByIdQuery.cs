// Application/Queries/Documents/GetPrivatePartyByIdQuery.cs
using MediatR;
using OMSV1.Application.Dtos.Documents;
using System;

namespace OMSV1.Application.Queries.Documents
{
    public class GetPrivatePartyByIdQuery : IRequest<PrivatePartyDto>
    {
        public Guid Id { get; }

        public GetPrivatePartyByIdQuery(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id must be a valid GUID.", nameof(id));

            Id = id;
        }
    }
}
