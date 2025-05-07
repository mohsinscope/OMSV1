// // Application/Queries/DocumentParties/GetDocumentPartiesByProjectAndTypeQueryHandler.cs
// using System.Collections.Generic;
// using System.Threading;
// using System.Threading.Tasks;
// using AutoMapper;
// using MediatR;
// using OMSV1.Application.Dtos.Documents;
// using OMSV1.Domain.Entities.Documents;
// using OMSV1.Domain.SeedWork;
// using OMSV1.Domain.Specifications.Documents;

// namespace OMSV1.Application.Queries.DocumentParties
// {
//     public class GetDocumentPartiesByProjectAndTypeQueryHandler
//         : IRequestHandler<GetDocumentPartiesByProjectAndTypeQuery, IEnumerable<DocumentPartyDto>>
//     {
//         private readonly IUnitOfWork _unitOfWork;
//         private readonly IMapper     _mapper;

//         public GetDocumentPartiesByProjectAndTypeQueryHandler(
//             IUnitOfWork unitOfWork,
//             IMapper mapper)
//         {
//             _unitOfWork = unitOfWork;
//             _mapper     = mapper;
//         }

//         public async Task<IEnumerable<DocumentPartyDto>> Handle(
//             GetDocumentPartiesByProjectAndTypeQuery request,
//             CancellationToken cancellationToken)
//         {
//             var spec = new DocumentPartyByProjectAndTypeSpecification(
//                 request.ProjectId,
//                 request.PartyType,
//                 request.IsOfficial);

//             var entities = await _unitOfWork
//                 .Repository<DocumentParty>()
//                 .ListAsync(spec);

//             return _mapper.Map<IEnumerable<DocumentPartyDto>>(entities);
//         }
//     }
// }
