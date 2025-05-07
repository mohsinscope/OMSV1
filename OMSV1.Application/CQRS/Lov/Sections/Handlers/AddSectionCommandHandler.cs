// Application/Handlers/Sections/AddSectionsCommandHandler.cs
using MediatR;
using OMSV1.Application.Commands.Sections;
using OMSV1.Application.Helpers;
using OMSV1.Domain.Entities.Sections;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Sections
{
public class AddSectionCommandHandler : IRequestHandler<AddSectionCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddSectionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(AddSectionCommand request, CancellationToken cancellationToken)
    {
        // 1) Load the Department to ensure it exists
        var department = await _unitOfWork.Repository<Department>()
            .FirstOrDefaultAsync(m => m.Id == request.DepartmentId);
        if (department is null)
            throw new HandlerException($"Department with Id '{request.DepartmentId}' not found.");

        // 2) Prevent duplicates *within that Section*
        var exists = await _unitOfWork.Repository<Section>()
            .FirstOrDefaultAsync(gd =>
                gd.Name == request.Name &&
                gd.DepartmentId == request.DepartmentId);
        if (exists != null)
            throw new HandlerException($"A Section named '{request.Name}' already exists under that Department.");

        // 3) Create and save
        var gd = new Section(request.Name, request.DepartmentId);
        await _unitOfWork.Repository<Section>().AddAsync(gd);

        if (!await _unitOfWork.SaveAsync(cancellationToken))
            throw new HandlerException("Failed to save Section.");

        return gd.Id;
    }
}

}
