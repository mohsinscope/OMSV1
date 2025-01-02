using MediatR;
using OMSV1.Application.Dtos;
namespace OMSV1.Application.Queries.DamagedPassports
{
    public class GetDamagedPassportByIdQuery : IRequest<DamagedPassportDto?>
    {
        public Guid Id { get; }

        public GetDamagedPassportByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
