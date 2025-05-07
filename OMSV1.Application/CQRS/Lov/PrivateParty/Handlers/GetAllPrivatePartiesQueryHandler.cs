// Application/Handlers/Documents/GetAllPrivatePartiesQueryHandler.cs
using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using OMSV1.Application.Dtos.Documents;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Documents;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Documents
{
    public class GetAllPrivatePartiesQueryHandler : IRequestHandler<GetAllPrivatePartiesQuery, PagedList<PrivatePartyDto>>
    {
        private readonly IGenericRepository<PrivateParty> _repository;
        private readonly IMapper _mapper;

        public GetAllPrivatePartiesQueryHandler(IGenericRepository<PrivateParty> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedList<PrivatePartyDto>> Handle(GetAllPrivatePartiesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var queryable = _repository.GetAllAsQueryable();
                var projected = queryable.ProjectTo<PrivatePartyDto>(_mapper.ConfigurationProvider);

                var paged = await PagedList<PrivatePartyDto>.CreateAsync(
                    projected,
                    request.PaginationParams.PageNumber,
                    request.PaginationParams.PageSize
                );

                return paged;
            }
            catch (Exception ex)
            {
                throw new HandlerException("An unexpected error occurred while retrieving Private Parties.", ex);
            }
        }
    }
}
