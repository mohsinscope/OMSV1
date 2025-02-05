using MediatR;
using OMSV1.Application.Commands.Reports;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Reports;
using OMSV1.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Handlers.Reports
{
    public class DeleteEmailReportCommandHandler : IRequestHandler<DeleteEmailReportCommand, bool>
    {
        private readonly IGenericRepository<EmailReport> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteEmailReportCommandHandler(IGenericRepository<EmailReport> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteEmailReportCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve the EmailReport to delete
                var emailReport = await _repository.GetByIdAsync(request.Id);
                if (emailReport == null)
                    throw new KeyNotFoundException($"EmailReport with ID {request.Id} not found.");

                // Delete the EmailReport asynchronously
                await _repository.DeleteAsync(emailReport);

                // Commit the changes to the database
                return await _unitOfWork.SaveAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new HandlerException("An error occurred while deleting the email report.", ex);
            }
        }
    }
}
