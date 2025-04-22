// Application/Handlers/Tags/AddTagsCommandHandler.cs
using AutoMapper;
using MediatR;
using OMSV1.Application.Commands.Tags;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Tags
{
    public class AddTagsCommandHandler : IRequestHandler<AddTagsCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddTagsCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(AddTagsCommand request, CancellationToken cancellationToken)
        {
            // Prevent duplicates in the same project
            var existing = await _unitOfWork.Repository<Tag>()
                .FirstOrDefaultAsync(dp =>
                    dp.Name == request.Name 
                    );

            if (existing != null)
                throw new HandlerException($"A Tag named '{request.Name}' already exists for this project.");

            // Create new Tag entity
            var tag = new Tag(
                request.Name
      );

            await _unitOfWork.Repository<Tag>()
                .AddAsync(tag);

            if (!await _unitOfWork.SaveAsync(cancellationToken))
                throw new HandlerException("Failed to save the Tag to the database.");

            return tag.Id;
        }
    }
}
