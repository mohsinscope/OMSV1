using MediatR;
using OMSV1.Application.Dtos.Documents;
using System;

namespace OMSV1.Application.Queries.Ministries
{
    public class GetMinistryByIdQuery : IRequest<MinistryDto>
    {
        public Guid Id { get; }

        public GetMinistryByIdQuery(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id must be a valid GUID.", nameof(id));

            Id = id;
        }
    }
}
