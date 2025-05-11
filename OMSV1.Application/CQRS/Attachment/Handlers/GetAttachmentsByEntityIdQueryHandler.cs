using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.Dtos;
using OMSV1.Application.Exceptions;
using OMSV1.Application.Queries.Attachments;
using OMSV1.Domain.Entities.Attachments;
using OMSV1.Domain.SeedWork;
using OMSV1.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OMSV1.Application.Helpers;

namespace OMSV1.Application.Handlers.Attachments
{
    public class GetAttachmentsByEntityIdQueryHandler 
        : IRequestHandler<GetAttachmentsByEntityIdQuery, List<AttachmentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAttachmentsByEntityIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<AttachmentDto>> Handle(
            GetAttachmentsByEntityIdQuery request, 
            CancellationToken cancellationToken)
        {
            try
            {
                IQueryable<AttachmentDto> dtoQuery;

                if (request.EntityType == EntityType.Document)
                {
                    // for Document attachments
                    dtoQuery = _unitOfWork
                        .Repository<DocumentAttachment>()
                        .GetAllAsQueryable()
                        .Where(da => da.DocumentId == request.EntityId)
                        .Select(da => new AttachmentDto
                        {
                            Id = da.Id,
                            EntityId = da.DocumentId,
                            EntityType = da.EntityType,
                            FilePath = da.FilePath
                        });
                }
                else
                {
                    // for all other “CU” attachments
                    dtoQuery = _unitOfWork
                        .Repository<AttachmentCU>()
                        .GetAllAsQueryable()
                        .Where(a => a.EntityId == request.EntityId
                                 && a.EntityType == request.EntityType)
                        .Select(a => new AttachmentDto
                        {
                            Id = a.Id,
                            EntityId = a.EntityId,
                            EntityType = a.EntityType,
                            FilePath = a.FilePath
                        });
                }

                return await dtoQuery.ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new HandlerException(
                    "An error occurred while retrieving attachments.", ex);
            }
        }
    }
}
