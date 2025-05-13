namespace OMSV1.Infrastructure.Interfaces;
public interface IDocumentLinkManager
{
    Task UpdateDocumentCcLinksAsync(Guid documentId, IEnumerable<Guid> ccIds, CancellationToken cancellationToken = default);
    Task UpdateDocumentTagLinksAsync(Guid documentId, IEnumerable<Guid> tagIds, CancellationToken cancellationToken = default);
}
