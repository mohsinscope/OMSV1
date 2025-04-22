using MediatR;
using OMSV1.Application.Commands.Tags;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Tags
{
    public class DeleteTagsCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteTagsCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<bool> Handle(DeleteTagsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve the Tag entity
                var tag = await _unitOfWork.Repository<Tag>().GetByIdAsync(request.Id);
                if (tag == null)
                {
                    throw new KeyNotFoundException($"Tag with ID {request.Id} not found.");
                }

                // Delete the entity
                await _unitOfWork.Repository<Tag>().DeleteAsync(tag);

                // Save changes to the database
                if (!await _unitOfWork.SaveAsync(cancellationToken))
                {
                    throw new Exception("Failed to delete the Tag from the database.");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new HandlerException("An error occurred while deleting the Tag.", ex);
            }
        }
    }
}
