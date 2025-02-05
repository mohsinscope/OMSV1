using MediatR;
using OMSV1.Application.Commands.Reports;
using OMSV1.Domain.Entities.Reports;
using OMSV1.Domain.SeedWork;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Handlers.Reports
{
    public class UpdateReportTypeCommandHandler : IRequestHandler<UpdateReportTypeCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateReportTypeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateReportTypeCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the existing ReportType entity asynchronously.
            var reportType = await _unitOfWork.Repository<ReportType>().GetByIdAsync(request.Id);
            if (reportType == null)
            {
                throw new Exception($"ReportType with id {request.Id} not found.");
            }

            // Update the entity with the new values.
            reportType.UpdateReportType(request.Name, request.Description);

            // Use the asynchronous update method.
            await _unitOfWork.Repository<ReportType>().UpdateAsync(reportType);

            // Save the changes to the database.
            var result = await _unitOfWork.SaveAsync(cancellationToken);
            return result;
        }
    }
}
