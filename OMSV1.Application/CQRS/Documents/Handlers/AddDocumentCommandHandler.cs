using AutoMapper;
using MediatR;
using OMSV1.Application.Commands.Documents;
using OMSV1.Domain.Entities.Attachments;
using OMSV1.Domain.Entities.DocumentHistories;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.Entities.Projects;
using OMSV1.Domain.Entities.Profiles;
using OMSV1.Domain.Enums;
using OMSV1.Domain.SeedWork;
using OMSV1.Application.Helpers;
using OMSV1.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

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
                // Retrieve required entities.
                var party = await _unitOfWork.Repository<DocumentParty>().GetByIdAsync(request.PartyId);
                if (party == null)
                    throw new KeyNotFoundException($"Document Party with ID {request.PartyId} was not found.");

                var profile = await _unitOfWork.Repository<OMSV1.Domain.Entities.Profiles.Profile>().GetByIdAsync(request.ProfileId);
                if (profile == null)
                    throw new KeyNotFoundException($"Profile with ID {request.ProfileId} was not found.");

                // Retrieve the CC DocumentParties if valid CCIds are provided.
                var ccParties = new List<DocumentParty>();
                if (request.CCIds != null && request.CCIds.Any())
                {
                    foreach (var ccId in request.CCIds)
                    {
                        if (ccId != Guid.Empty)
                        {
                            var ccParty = await _unitOfWork.Repository<DocumentParty>().GetByIdAsync(ccId);
                            if (ccParty != null)
                                ccParties.Add(ccParty);
                        }
                    }
                }

                // Create the document using client-supplied values.
                var document = new Document(
                    documentNumber: request.DocumentNumber,
                    title: request.Title,
                    docType: request.DocumentType,
                    projectId: request.ProjectId,
                    documentDate: DateTime.SpecifyKind(request.DocumentDate, DateTimeKind.Utc),
                    requiresReply: request.IsRequiresReply,
                    partyId: request.PartyId,
                    party: party,
                    profileId: request.ProfileId,
                    profile: profile,
                    subject: request.Subject,
                    parentDocumentId: request.ParentDocumentId,
                    ccs: ccParties,
                    responseType: request.ResponseType,  // Value supplied by client
                    notes: request.Notes                 // NEW: Optional notes value
                );

                // Add the document to the repository.
                await _unitOfWork.Repository<Document>().AddAsync(document);

                // Create a DocumentHistory entry for the Add action.
                var history = new DocumentHistory(
                    documentId: document.Id,
                    profileId: request.ProfileId,
                    actionType: DocumentActions.Add,
                    actionDate: DateTime.UtcNow,
                    notes: "Document created with attachment."
                );
                await _unitOfWork.Repository<DocumentHistory>().AddAsync(history);

                // Process file attachments.
                if (request.File == null || request.File.Count == 0)
                    throw new ArgumentException("No files were uploaded for the attachments.");

                foreach (var file in request.File)
                {
                    if (file != null && file.Length > 0)
                    {
                        var photoResult = await _photoService.AddPhotoAsync(file, document.Id, EntityType.Document);
                        var attachment = new AttachmentCU(
                            filePath: photoResult.FilePath,
                            entityType: EntityType.Document,
                            entityId: document.Id
                        );
                        await _unitOfWork.Repository<AttachmentCU>().AddAsync(attachment);
                    }
                }

                // Commit changes in one transaction.
                if (!await _unitOfWork.SaveAsync(cancellationToken))
                {
                    throw new Exception("Failed to save the document and its attachments to the database.");
                }

                return document.Id;
            }
            catch (Exception ex)
            {
                throw new HandlerException("An error occurred while creating the document with attachment.", ex);
            }
        }
    }
}
