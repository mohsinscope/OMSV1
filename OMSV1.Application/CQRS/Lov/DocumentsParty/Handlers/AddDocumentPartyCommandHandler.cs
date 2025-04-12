using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using OMSV1.Application.Commands.DocumentParties;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.DocumentParties
{
    public class AddDocumentPartyCommandHandler : IRequestHandler<AddDocumentPartyCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddDocumentPartyCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(AddDocumentPartyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Check for duplicate DocumentParty name
                var existingParty = await _unitOfWork.Repository<DocumentParty>()
                    .FirstOrDefaultAsync(dp => dp.Name == request.Name);

                if (existingParty != null)
                    throw new HandlerException($"A DocumentParty with the name '{request.Name}' already exists.");

                // Option 1: Use direct instantiation if no mapping is needed
                var documentParty = new DocumentParty(request.Name);
                // Option 2: If you prefer using AutoMapper, you can map the command to the entity:
                // var documentParty = _mapper.Map<DocumentParty>(request);

                await _unitOfWork.Repository<DocumentParty>().AddAsync(documentParty);

                if (!await _unitOfWork.SaveAsync(cancellationToken))
                    throw new HandlerException("Failed to save the DocumentParty to the database.");

                return documentParty.Id;
            }
            catch (HandlerException ex)
            {
                throw new HandlerException("An error occurred while creating the DocumentParty.", ex);
            }
            catch (Exception ex)
            {
                throw new HandlerException("An unexpected error occurred.", ex);
            }
        }
    }
}
