// // Query
// using MediatR;
// using OMSV1.Application.Dtos;
// using OMSV1.Domain.SeedWork;

// public record GetAttachmentByIdQuery(Guid AttachmentId) : IRequest<AttachmentDto>;

// // Handler
// public sealed class GetAttachmentByIdQueryHandler
//        : IRequestHandler<GetAttachmentByIdQuery, AttachmentDto>
// {
//     private readonly IGenericRepository<Attachment> _repo;

//     public GetAttachmentByIdQueryHandler(IGenericRepository<Attachment> repo)
//     {
//         _repo = repo;
//     }
//         public class AttachmentDto
//     {
//         public Guid   Id          { get; init; }
//         public string FilePath    { get; init; } = string.Empty;
//         public string ContentType { get; init; } = "application/octet-stream";
//         public string FileName    { get; init; } = string.Empty;
//     }

//     public async Task<AttachmentDto?> Handle(
//         GetAttachmentByIdQuery request, CancellationToken ct)
//     {
//         var entity = await _repo.GetByIdAsync(request.AttachmentId);
//         if (entity == null) return null;

//         return new AttachmentDto
//         {
//             Id          = entity.Id,
//             FilePath    = entity.FilePath,
//             ContentType = entity.ContentType,
//             FileName    = entity.FileName
//         };
//     }
// }
