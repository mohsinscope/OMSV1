// Application/Handlers/GeneralDirectorates/AddGeneralDirectorateCommandHandler.cs
using AutoMapper;
using MediatR;
using OMSV1.Application.Commands.GeneralDirectorates;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.GeneralDirectorates;
using OMSV1.Domain.Entities.Ministries;
using OMSV1.Domain.SeedWork;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Handlers.GeneralDirectorates
{
public class AddGeneralDirectorateCommandHandler : IRequestHandler<AddGeneralDirectorateCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddGeneralDirectorateCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(AddGeneralDirectorateCommand request, CancellationToken cancellationToken)
    {
        // 1) Load the ministry to ensure it exists
        var ministry = await _unitOfWork.Repository<Ministry>()
            .FirstOrDefaultAsync(m => m.Id == request.MinistryId);
        if (ministry is null)
            throw new HandlerException($"Ministry with Id '{request.MinistryId}' not found.");

        // 2) Prevent duplicates *within that ministry*
        var exists = await _unitOfWork.Repository<GeneralDirectorate>()
            .FirstOrDefaultAsync(gd =>
                gd.Name == request.Name &&
                gd.MinistryId == request.MinistryId);
        if (exists != null)
            throw new HandlerException($"A GeneralDirectorate named '{request.Name}' already exists under that ministry.");

        // 3) Create and save
        var gd = new GeneralDirectorate(request.Name, request.MinistryId);
        await _unitOfWork.Repository<GeneralDirectorate>().AddAsync(gd);

        if (!await _unitOfWork.SaveAsync(cancellationToken))
            throw new HandlerException("Failed to save GeneralDirectorate.");

        return gd.Id;
    }
}

}
