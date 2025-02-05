using AutoMapper;
using MediatR;
using OMSV1.Application.DTOs.Reports;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Reports;
using OMSV1.Domain.Entities.Reports;
using OMSV1.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Handlers.Reports
{
    public class GetEmailReportByIdQueryHandler : IRequestHandler<GetEmailReportByIdQuery, EmailReportDto>
    {
        private readonly IGenericRepository<EmailReport> _repository;
        private readonly IMapper _mapper;

        public GetEmailReportByIdQueryHandler(IGenericRepository<EmailReport> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<EmailReportDto> Handle(GetEmailReportByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Fetch the EmailReport with ReportTypes included
                var emailReport = await _repository.GetByIdWithIncludesAsync(request.Id, er => er.ReportTypes);

                // Throw if not found
                if (emailReport == null)
                    throw new KeyNotFoundException($"EmailReport with ID {request.Id} not found.");

                // Map EmailReport entity to EmailReportDto
                return _mapper.Map<EmailReportDto>(emailReport);
            }
            catch (Exception ex)
            {
                // Handle the exception and throw a custom HandlerException for better error reporting
                throw new HandlerException("An error occurred while fetching the email report.", ex);
            }
        }
    }
}
