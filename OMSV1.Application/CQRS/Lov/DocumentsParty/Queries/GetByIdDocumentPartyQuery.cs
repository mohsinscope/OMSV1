using MediatR;
using OMSV1.Application.Dtos.Documents;
using System;

namespace OMSV1.Application.Queries.DocumentParties
{
    public class GetDocumentPartyByIdQuery : IRequest<DocumentPartyDto>
    {
        public Guid Id { get; set; }
    }
}
