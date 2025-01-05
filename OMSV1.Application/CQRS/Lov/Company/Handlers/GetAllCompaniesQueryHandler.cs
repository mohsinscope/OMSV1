using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using OMSV1.Application.Dtos.Companies;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Companies;
using OMSV1.Domain.Entities.Companies;
using OMSV1.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class GetAllCompaniesQueryHandler : IRequestHandler<GetAllCompaniesQuery, PagedList<CompanyDto>>
{
    private readonly IGenericRepository<Company> _repository;
    private readonly IMapper _mapper;

    public GetAllCompaniesQueryHandler(IGenericRepository<Company> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PagedList<CompanyDto>> Handle(GetAllCompaniesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Retrieve the companies as IQueryable with included LectureTypes
            var companiesQuery = _repository.GetAllAsQueryable()
                .Include(c => c.LectureTypes); // Eagerly load the related LectureTypes

            // Map the result to CompanyDto using AutoMapper's ProjectTo
            var mappedQuery = companiesQuery.ProjectTo<CompanyDto>(_mapper.ConfigurationProvider);

            // Apply pagination using PagedList
            var pagedCompanies = await PagedList<CompanyDto>.CreateAsync(
                mappedQuery,
                request.PaginationParams.PageNumber,
                request.PaginationParams.PageSize
            );

            return pagedCompanies;
        }
        catch (Exception ex)
        {
            // Catch and throw a custom exception for better error reporting
            throw new HandlerException("An error occurred while retrieving companies.", ex);
        }
    }
}
