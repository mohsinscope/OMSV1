using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos.Directorates;
using OMSV1.Domain.Entities.Directorates;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Queries.Directorates
{
    public class GetDirectorateByIdQueryHandler : IRequestHandler<GetDirectorateyByIdQuery, DirectorateDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetDirectorateByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DirectorateDto> Handle(GetDirectorateyByIdQuery request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Directorate>();

            var generalDirectorate = await repo
                .GetAllAsQueryable()                                            // get the IQueryable<Directorate>
                .Include(gd => gd.GeneralDirectorate)                        // include the GeneralDirectorate nav prop
                .FirstOrDefaultAsync(gd => gd.Id == request.Id, cancellationToken);

            if (generalDirectorate == null)
                throw new KeyNotFoundException($"Directorate with ID {request.Id} was not found.");

            return new DirectorateDto
            {
                Id           = generalDirectorate.Id,
                Name         = generalDirectorate.Name,
                GeneralDirectorateId  = generalDirectorate.GeneralDirectorateId,
                GeneralDirectorateName = generalDirectorate.GeneralDirectorate.Name,
                DateCreated  = generalDirectorate.DateCreated
            };
        }
    }
}
