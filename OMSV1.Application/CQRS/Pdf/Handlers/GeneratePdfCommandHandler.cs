// using MediatR;
// using OMSV1.Application.Pdf.Commands;
// using OMSV1.Infrastructure.Interfaces;

// namespace OMSV1.Application.Pdf.Handlers
// {
//     public class GeneratePdfCommandHandler : IRequestHandler<GeneratePdfCommand, GeneratePdfResult>
//     {
//         private readonly IPdfService _pdfService;

//         public GeneratePdfCommandHandler(IPdfService pdfService)
//         {
//             _pdfService = pdfService;
//         }

//         public async Task<GeneratePdfResult> Handle(GeneratePdfCommand command, CancellationToken cancellationToken)
//         {
//             var filePath = await _pdfService.GenerateAsync(
//                 command.Title,
//                 command.Content,
//                 command.Metadata
//             );

//             return new GeneratePdfResult
//             {
//                 FilePath = filePath,
//                 GeneratedAt = DateTime.UtcNow
//             };
//         }
//     }
// }
