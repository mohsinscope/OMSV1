// Application/Handlers/Documents/GetPrivatePartyByIdQueryHandler.cs
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos.Documents;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Documents;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Documents
{
    public class GetPrivatePartyByIdQueryHandler : IRequestHandler<GetPrivatePartyByIdQuery, PrivatePartyDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPrivatePartyByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PrivatePartyDto> Handle(GetPrivatePartyByIdQuery request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<PrivateParty>();

            var party = await repo
                .GetAllAsQueryable()
                .Include(pp => pp.Documents)
                .FirstOrDefaultAsync(pp => pp.Id == request.Id, cancellationToken);

            if (party == null)
                throw new KeyNotFoundException($"PrivateParty with ID {request.Id} was not found.");

            return new PrivatePartyDto
            {
                Id            = party.Id,
                Name          = party.Name,
               // DocumentCount = party.Documents.Count,
                DateCreated   = party.DateCreated
            };
        }
    }
}
