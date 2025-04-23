using MediatR;
using OMSV1.Application.Dtos.Documents;
using System;

namespace OMSV1.Application.Queries.DocumentCC
{
    public class GetDocumentCCByIdQuery : IRequest<DocumentCCDto>
    {
        public Guid Id { get; }

        public GetDocumentCCByIdQuery(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id must be a valid GUID.", nameof(id));

            Id = id;
        }
    }
}
