using AutoMapper;
using MediatR;
using OMSV1.Application.Commands.Documents;
using OMSV1.Domain.Entities.Attachments;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.Entities.Projects;
using OMSV1.Domain.SeedWork;
using OMSV1.Application.Helpers;
using OMSV1.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Handlers.Documents
{
    public class AddDocumentWithAttachmentCommandHandler : IRequestHandler<AddDocumentWithAttachmentCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public AddDocumentWithAttachmentCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IPhotoService photoService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _photoService = photoService;
        }

        public async Task<Guid> Handle(AddDocumentWithAttachmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
            // Retrieve the sender DocumentParty (required)
            var party = await _unitOfWork.Repository<DocumentParty>()
                .GetByIdAsync(request.PartyId);
            if (party == null)
            {
                throw new KeyNotFoundException($"Document Party with ID {request.PartyId} was not found.");
            }

            // Retrieve the CC DocumentParty if provided. If none is provided, leave ccParty null.
            DocumentParty? ccParty = null;
            if (request.CCId.HasValue && request.CCId.Value != Guid.Empty)
            {
                ccParty = await _unitOfWork.Repository<DocumentParty>()
                    .GetByIdAsync(request.CCId.Value);
                // Optionally, validate if ccParty is null and decide whether to throw an error.
            }

            // Create the document. Note that we pass request.CCId as is – if no CC is provided, it should be null.
            var document = new Document(
                documentNumber: request.DocumentNumber,
                title: request.Title,
                docType: request.DocumentType,
                projectId: request.ProjectId,
                documentDate: DateTime.SpecifyKind(request.DocumentDate, DateTimeKind.Utc),
                requiresReply: request.IsRequiresReply,
                partyId: request.PartyId,
                party: party,
                subject: request.Subject,
                parentDocumentId: request.ParentDocumentId,
                ccId: request.CCId,   // This should be null if not provided.
                cc: ccParty
            );

                // 4. Add the document using the repository
                await _unitOfWork.Repository<Document>().AddAsync(document);

                // 5. Validate file attachments
                if (request.File == null || request.File.Count == 0)
                {
                    throw new ArgumentException("No files were uploaded for the attachments.");
                }

                // 6. For each attachment file, upload and store the attachment
                foreach (var file in request.File)
                {
                    if (file?.Length > 0)
                    {
                        // Example: Upload file, associating it with the Document
                        var photoResult = await _photoService.AddPhotoAsync(file, document.Id, OMSV1.Domain.Enums.EntityType.Document);
                        
                        // Create a new AttachmentCU for this file.
                        var attachment = new AttachmentCU(
                            filePath: photoResult.FilePath,
                            entityType: OMSV1.Domain.Enums.EntityType.Document,
                            entityId: document.Id
                        );

                        await _unitOfWork.Repository<AttachmentCU>().AddAsync(attachment);
                    }
                }

                // 7. Save all changes in one transaction
                if (!await _unitOfWork.SaveAsync(cancellationToken))
                {
                    throw new Exception("Failed to save the document and its attachments to the database.");
                }

                // Return the newly created document's ID
                return document.Id;
            }
            catch (Exception ex)
            {
                throw new HandlerException("An error occurred while creating the document with attachment.", ex);
            }
        }
    }
}
