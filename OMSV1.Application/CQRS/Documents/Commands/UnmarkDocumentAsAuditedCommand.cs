// --- UnmarkDocumentAsAuditedCommand.cs ---
using MediatR;
using System;

namespace OMSV1.Application.Commands.Documents
{
    public class UnmarkDocumentAsAuditedCommand : IRequest<bool>
    {
        public Guid DocumentId { get; set; }
        public Guid ProfileId  { get; set; }
    }
}
