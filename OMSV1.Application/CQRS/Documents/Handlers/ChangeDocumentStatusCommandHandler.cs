// using MediatR;
// using OMSV1.Application.Commands.Documents;
// using OMSV1.Domain.Entities.DocumentHistories;
// using OMSV1.Domain.Entities.Documents;
// using OMSV1.Domain.Enums;
// using OMSV1.Domain.SeedWork;
// using OMSV1.Application.Helpers;
// using System;
// using System.Collections.Generic;
// using System.Threading;
// using System.Threading.Tasks;

// namespace OMSV1.Application.Handlers.Documents
// {
//     public class ChangeDocumentStatusCommandHandler : IRequestHandler<ChangeDocumentStatusCommand, bool>
//     {
//         private readonly IUnitOfWork _unitOfWork;

//         public ChangeDocumentStatusCommandHandler(IUnitOfWork unitOfWork)
//         {
//             _unitOfWork = unitOfWork;
//         }

//         public async Task<bool> Handle(ChangeDocumentStatusCommand request, CancellationToken cancellationToken)
//         {
//             try
//             {
//                 // Retrieve the Document
//                 var document = await _unitOfWork.Repository<Document>().GetByIdAsync(request.DocumentId);
//                 if (document == null)
//                     throw new KeyNotFoundException($"Document with ID {request.DocumentId} not found.");

//                 // Use the domain method to mark that it no longer requires a reply.
//                 document.MarkNoLongerRequiresReply();

//                 // Add a DocumentHistory record for the status change action.
//                 var history = new DocumentHistory(
//                     documentId: document.Id,
//                     userId: request.UserId,
//                     actionType: DocumentActions.ChangeStatus,
//                     actionDate: DateTime.UtcNow,
//                     notes: request.Notes
//                 );
//                 await _unitOfWork.Repository<DocumentHistory>().AddAsync(history);

//                 // Save changes
//                 if (!await _unitOfWork.SaveAsync(cancellationToken))
//                     throw new Exception("Failed to change the document status in the database.");

//                 return true;
//             }
//             catch (Exception ex)
//             {
//                 throw new HandlerException("An error occurred while changing the document status.", ex);
//             }
//         }
//     }
// }
