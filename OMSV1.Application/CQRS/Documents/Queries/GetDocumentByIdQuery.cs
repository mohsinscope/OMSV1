// Application/Queries/Documents/GetDocumentByIdDetailedQuery.cs
using MediatR;
using OMSV1.Application.Dtos.Documents;

public class GetDocumentByIdDetailedQuery : IRequest<DocumentDetailedDto>
{
    public Guid Id { get; }
    public int Depth { get; }

    public GetDocumentByIdDetailedQuery(Guid id, int depth)
    {
        Id = id;
        Depth = depth;
    }
}
