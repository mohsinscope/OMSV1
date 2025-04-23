// Application/Handlers/Ministry/AddMinistryCommandHandler.cs
using AutoMapper;
using MediatR;
using OMSV1.Application.Commands.Ministries;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Ministries;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Ministries
{
    public class AddMinistryCommandHandler : IRequestHandler<AddMinistryCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddMinistryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(AddMinistryCommand request, CancellationToken cancellationToken)
        {
            // Prevent duplicates in the same project
            var existing = await _unitOfWork.Repository<Ministry>()
                .FirstOrDefaultAsync(dp =>
                    dp.Name == request.Name 
                    );

            if (existing != null)
                throw new HandlerException($"A Ministry named '{request.Name}' already exists for this project.");

            // Create new Ministry entity
            var ministry = new Ministry(
                request.Name
      );

            await _unitOfWork.Repository<Ministry>()
                .AddAsync(ministry);

            if (!await _unitOfWork.SaveAsync(cancellationToken))
                throw new HandlerException("Failed to save the Ministry to the database.");

            return ministry.Id;
        }
    }
}
