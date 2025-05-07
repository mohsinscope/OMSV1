// Application/Handlers/Directorates/AddDirectorateCommandHandler.cs
using AutoMapper;
using MediatR;
using OMSV1.Application.Commands.Directorates;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Directorates;
using OMSV1.Domain.Entities.GeneralDirectorates;
using OMSV1.Domain.Entities.Ministries;
using OMSV1.Domain.SeedWork;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Handlers.Directorates
{
public class AddDirectorateCommandHandler : IRequestHandler<AddDirectorateCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddDirectorateCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(AddDirectorateCommand request, CancellationToken cancellationToken)
    {
        // 1) Load the ministry to ensure it exists
        var ministry = await _unitOfWork.Repository<GeneralDirectorate>()
            .FirstOrDefaultAsync(m => m.Id == request.GeneralDirectorateId);
        if (ministry is null)
            throw new HandlerException($"GeneralDirectorate with Id '{request.GeneralDirectorateId}' not found.");

        // 2) Prevent duplicates *within that ministry*
        var exists = await _unitOfWork.Repository<Directorate>()
            .FirstOrDefaultAsync(gd =>
                gd.Name == request.Name &&
                gd.GeneralDirectorateId == request.GeneralDirectorateId);
        if (exists != null)
            throw new HandlerException($"A Directorate named '{request.Name}' already exists under that ministry.");

        // 3) Create and save
        var gd = new Directorate(request.Name, request.GeneralDirectorateId);
        await _unitOfWork.Repository<Directorate>().AddAsync(gd);

        if (!await _unitOfWork.SaveAsync(cancellationToken))
            throw new HandlerException("Failed to save Directorate.");

        return gd.Id;
    }
}

}
