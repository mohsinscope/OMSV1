using MediatR;
using OMSV1.Application.Dtos.Sections;

namespace OMSV1.Application.Queries.Sections
{
    public class GetSectionsByIdQuery : IRequest<SectionDto>
    {
        public Guid Id { get; }

        public GetSectionsByIdQuery(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id must be a valid GUID.", nameof(id));

            Id = id;
        }
    }
}
