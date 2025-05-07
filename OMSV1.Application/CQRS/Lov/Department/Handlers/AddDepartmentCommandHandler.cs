// Application/Handlers/Directorates/AddDirectorateCommandHandler.cs
using MediatR;
using OMSV1.Application.Commands.Department;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Directorates;
using OMSV1.Domain.Entities.Sections;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Departments
{
public class AddDepartmentCommandHandler : IRequestHandler<AddDepartmentCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddDepartmentCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(AddDepartmentCommand request, CancellationToken cancellationToken)
    {
        // 1) Load the directorate to ensure it exists
        var directorate = await _unitOfWork.Repository<Directorate>()
            .FirstOrDefaultAsync(m => m.Id == request.DirectorateId);
        if (directorate is null)
            throw new HandlerException($"Directorate with Id '{request.DirectorateId}' not found.");

        // 2) Prevent duplicates *within that Department*
        var exists = await _unitOfWork.Repository<Department>()
            .FirstOrDefaultAsync(gd =>
                gd.Name == request.Name &&
                gd.DirectorateId == request.DirectorateId);
        if (exists != null)
            throw new HandlerException($"A Department named '{request.Name}' already exists under that Directorate.");

        // 3) Create and save
        var gd = new Department(request.Name, request.DirectorateId);
        await _unitOfWork.Repository<Department>().AddAsync(gd);

        if (!await _unitOfWork.SaveAsync(cancellationToken))
            throw new HandlerException("Failed to save Department.");

        return gd.Id;
    }
}

}
