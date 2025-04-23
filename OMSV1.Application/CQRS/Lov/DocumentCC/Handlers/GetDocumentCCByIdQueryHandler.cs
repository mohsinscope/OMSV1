using MediatR;
using OMSV1.Application.Dtos.Documents;
using OMSV1.Application.Queries.DocumentCC;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Application.Handlers.Documentcc
{
    public class GetDocumentCCByIdQueryHandler : IRequestHandler<GetDocumentCCByIdQuery, DocumentCCDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetDocumentCCByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DocumentCCDto> Handle(GetDocumentCCByIdQuery request, CancellationToken cancellationToken)
        {
            var documentCC = await _unitOfWork.Repository<DocumentCC>()
                .GetByIdAsync(request.Id);

            if (documentCC == null)
                throw new KeyNotFoundException($"Document CC with ID {request.Id} was not found.");

            return new DocumentCCDto
            {
                Id = documentCC.Id,
                RecipientName = documentCC.RecipientName
  
            };
        }
    }
}

