using MediatR;
using OMSV1.Application.Dtos.Directorates;

namespace OMSV1.Application.Queries.Directorates
{
    public class GetDirectorateyByIdQuery : IRequest<DirectorateDto>
    {
        public Guid Id { get; }

        public GetDirectorateyByIdQuery(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id must be a valid GUID.", nameof(id));

            Id = id;
        }
    }
}
