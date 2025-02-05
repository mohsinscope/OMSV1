using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using OMSV1.Application.DTOs.Reports; // Contains EmailReportDto and ReportTypeDto
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Reports; // Contains GetAllEmailReportsQuery
using OMSV1.Domain.Entities.Reports; // Contains EmailReport entity
using OMSV1.Domain.SeedWork;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Handlers.Reports
{
    public class GetAllEmailReportsQueryHandler : IRequestHandler<GetAllEmailReportsQuery, PagedList<EmailReportDto>>
    {
        private readonly IGenericRepository<EmailReport> _repository;
        private readonly IMapper _mapper;

        public GetAllEmailReportsQueryHandler(IGenericRepository<EmailReport> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedList<EmailReportDto>> Handle(GetAllEmailReportsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve the EmailReports as IQueryable.
                // If your repository supports eager-loading, you can include ReportTypes as shown below:
                // var emailReportsQuery = _repository.GetAllAsQueryable(include: query => query.Include(er => er.ReportTypes));
                var emailReportsQuery = _repository.GetAllAsQueryable();

                // Map to EmailReportDto using AutoMapper's ProjectTo extension method.
                var mappedQuery = emailReportsQuery.ProjectTo<EmailReportDto>(_mapper.ConfigurationProvider);

                // Apply pagination using the PagedList helper.
                var pagedEmailReports = await PagedList<EmailReportDto>.CreateAsync(
                    mappedQuery,
                    request.PaginationParams.PageNumber,
                    request.PaginationParams.PageSize
                );

                return pagedEmailReports;
            }
            catch (Exception ex)
            {
                throw new HandlerException("An error occurred while retrieving email reports.", ex);
            }
        }
    }
}
