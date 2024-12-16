using MediatR;
using OMSV1.Domain.Entities.DamagedPassport;

namespace OMSV1.Application.Queries.DamagedPassports
{
    public class GetDamagedPassportByIdQuery : IRequest<DamagedPassport?>
    {
        public int Id { get; }

        public GetDamagedPassportByIdQuery(int id)
        {
            Id = id;
        }
    }
}
