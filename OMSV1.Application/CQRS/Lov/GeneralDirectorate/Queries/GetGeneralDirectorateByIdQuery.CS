using MediatR;
using OMSV1.Application.Dtos.GeneralDirectorates;

namespace OMSV1.Application.Queries.GeneralDirectorates
{
    public class GetGeneralDirectorateyByIdQuery : IRequest<GeneralDirectorateDto>
    {
        public Guid Id { get; }

        public GetGeneralDirectorateyByIdQuery(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id must be a valid GUID.", nameof(id));

            Id = id;
        }
    }
}
