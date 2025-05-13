using Microsoft.EntityFrameworkCore;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Infrastructure.Interfaces;
using OMSV1.Infrastructure.Persistence;

public class DocumentLinkManager : IDocumentLinkManager
{
    private readonly AppDbContext _dbContext;

    public DocumentLinkManager(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task UpdateDocumentCcLinksAsync(Guid documentId, IEnumerable<Guid> ccIds, CancellationToken cancellationToken = default)
    {
        // 1. Remove existing links
        var existingLinks = await _dbContext.Set<DocumentCcLink>()
            .Where(link => link.DocumentId == documentId)
            .ToListAsync(cancellationToken);

        if (existingLinks.Any())
        {
            _dbContext.Set<DocumentCcLink>().RemoveRange(existingLinks);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        // 2. Add new links
        if (ccIds.Any())
        {
            var newLinks = ccIds.Select(ccId => new DocumentCcLink(documentId, ccId));
            await _dbContext.Set<DocumentCcLink>().AddRangeAsync(newLinks, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task UpdateDocumentTagLinksAsync(Guid documentId, IEnumerable<Guid> tagIds, CancellationToken cancellationToken = default)
    {
        // 1. Remove existing links
        var existingLinks = await _dbContext.Set<DocumentTagLink>()
            .Where(link => link.DocumentId == documentId)
            .ToListAsync(cancellationToken);

        if (existingLinks.Any())
        {
            _dbContext.Set<DocumentTagLink>().RemoveRange(existingLinks);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        // 2. Add new links
        if (tagIds.Any())
        {
            var newLinks = tagIds.Select(tagId => new DocumentTagLink(documentId, tagId));
            await _dbContext.Set<DocumentTagLink>().AddRangeAsync(newLinks, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}