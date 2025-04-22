using MediatR;
using OMSV1.Application.Dtos.Documents;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Queries.Tags
{
    public class GetTagsByIdQueryHandler : IRequestHandler<GetTagsByIdQuery, TagsDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTagsByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TagsDto> Handle(GetTagsByIdQuery request, CancellationToken cancellationToken)
        {
            var tag = await _unitOfWork.Repository<Tag>()
                .GetByIdAsync(request.Id);

            if (tag == null)
                throw new KeyNotFoundException($"Tag with ID {request.Id} was not found.");

            return new TagsDto
            {
                Id = tag.Id,
                Name = tag.Name,
                DateCreated = tag.DateCreated
            };
        }
    }
}

