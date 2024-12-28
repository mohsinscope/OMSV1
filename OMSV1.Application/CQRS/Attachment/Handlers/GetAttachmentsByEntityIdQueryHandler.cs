using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos;
using OMSV1.Application.Dtos.Attachments;
using OMSV1.Application.Helpers;
using OMSV1.Application.Queries.Attachments;
using OMSV1.Domain.Entities.Attachments;
using OMSV1.Domain.SeedWork;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OMSV1.Application.Handlers.Attachments
{
    public class GetAttachmentsByEntityIdQueryHandler : IRequestHandler<GetAttachmentsByEntityIdQuery, List<AttachmentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAttachmentsByEntityIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<AttachmentDto>> Handle(GetAttachmentsByEntityIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Fetch attachments by entity ID and type using IUnitOfWork repository
                var attachmentsQuery = _unitOfWork.Repository<AttachmentCU>().GetAllAsQueryable()
                    .Where(a => a.EntityId == request.EntityId && a.EntityType == request.EntityType)
                    .Select(a => new AttachmentDto
                    {
                        Id = a.Id,
                        EntityId = a.EntityId,
                        EntityType = a.EntityType,
                        FileName = a.FileName,
                        FilePath = a.FilePath // Return the file path (URL) to the client
                    });

                // Execute the query and return the list of attachments
                return await attachmentsQuery.ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                // Handle exceptions by throwing custom HandlerException
                throw new HandlerException("An error occurred while retrieving attachments.", ex);
            }
        }
    }
}
