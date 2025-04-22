// Application/Handlers/DocumentCC/AddDocumentCCCommandHandler.cs
using AutoMapper;
using MediatR;
using OMSV1.Application.Commands.DocumentCC;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Documentcc
{
    public class AddDocumentCCCommandHandler : IRequestHandler<AddDocumentCCCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddDocumentCCCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(AddDocumentCCCommand request, CancellationToken cancellationToken)
        {
            // Prevent duplicates in the same project
            var existing = await _unitOfWork.Repository<DocumentCC>()
                .FirstOrDefaultAsync(dp =>
                    dp.RecipientName == request.RecipientName 
                    );

            if (existing != null)
                throw new HandlerException($"A CC named '{request.RecipientName}' already exists for this project.");

            // Create new DocumentCC entity
            var documentCC = new DocumentCC(
                request.RecipientName
);

            await _unitOfWork.Repository<DocumentCC>()
                .AddAsync(documentCC);

            if (!await _unitOfWork.SaveAsync(cancellationToken))
                throw new HandlerException("Failed to save the DocumentCC to the database.");

            return documentCC.Id;
        }
    }
}
