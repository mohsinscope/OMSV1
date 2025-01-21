using MediatR;

namespace OMSV1.Application.Pdf.Commands
{
    public class GeneratePdfCommand : IRequest<GeneratePdfResult>
    {
        public required string Title { get; set; }
        public required string Content { get; set; }
        public required Dictionary<string, string> Metadata { get; set; }
    }

    public class GeneratePdfResult
    {
        public required string FilePath { get; set; }
        public DateTime GeneratedAt { get; set; }
    }
}