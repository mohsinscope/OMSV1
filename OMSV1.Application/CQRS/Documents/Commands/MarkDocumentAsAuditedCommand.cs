using MediatR;
using System;

namespace OMSV1.Application.Commands.Documents
{
    public class MarkDocumentAsAuditedCommand : IRequest<bool>
    {
        public Guid DocumentId { get; set; }
    }
}
