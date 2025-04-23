using MediatR;
using OMSV1.Application.Dtos.Documents;

namespace OMSV1.Application.Queries.Tags
{
    public class GetTagsByIdQuery : IRequest<TagsDto>
    {
        public Guid Id { get; }

        public GetTagsByIdQuery(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id must be a valid GUID.", nameof(id));

            Id = id;
        }
    }
}
