using Microsoft.EntityFrameworkCore;
using TransitiveClosureTable.Domain.Entities;
using TransitiveClosureTable.Infrastructure.Data.Repositories.Contracts;

namespace TransitiveClosureTable.Infrastructure.Data.Repositories;

/// <summary>
/// Repository for managing TransitiveClosure entities.
/// </summary>
public class TransitiveClosureRepository(AppDbContext appDbContext) : ITransitiveClosureRepository
{
    /// <summary>
    /// Gets all ancestor closures of a given node.
    /// </summary>
    /// <param name="descendantId">The node ID for which to find ancestors.</param>
    /// <returns>List of TransitiveClosure entries representing ancestors.</returns>
    public async Task<List<TransitiveClosure>> GetAncestorsAsync(int descendantId)
    {
        return await appDbContext.TransitiveClosures
            .Where(tc => tc.DescendantId == descendantId)
            .OrderBy(tc => tc.Depth) // optional: closest ancestor first
            .ToListAsync();
    }

    /// <summary>
    /// Gets all transitive closure rows for a specific tree.
    /// </summary>
    /// <param name="treeId">The tree ID.</param>
    /// <returns>List of TransitiveClosure entries belonging to the tree.</returns>
    public async Task<List<TransitiveClosure>> GetByTreeIdAsync(int treeId)
    {
        return await appDbContext.TransitiveClosures
            .Where(tc => tc.TreeId == treeId)
            .ToListAsync();
    }

    /// <summary>
    /// Deletes a single transitive closure row.
    /// </summary>
    /// <param name="closure">The closure row to delete. If null, nothing happens.</param>
    public async Task DeleteAsync(TransitiveClosure? closure)
    {
        if (closure == null)
            return;

        appDbContext.TransitiveClosures.Remove(closure);
        await appDbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Gets all closure rows that reference a specific node, either as ancestor or descendant.
    /// </summary>
    /// <param name="nodeId">The node ID.</param>
    /// <returns>List of related TransitiveClosure entries.</returns>
    public async Task<List<TransitiveClosure>> GetAllByNodeIdAsync(int nodeId)
    {
        return await appDbContext.TransitiveClosures
            .Where(tc => tc.DescendantId == nodeId || tc.AncestorId == nodeId)
            .ToListAsync();
    }

    /// <summary>
    /// Deletes multiple transitive closure rows at once.
    /// </summary>
    /// <param name="transitiveClosures">Collection of closures to delete.</param>
    public async Task RemoveRangeAsync(IEnumerable<TransitiveClosure> transitiveClosures)
    {
        appDbContext.TransitiveClosures.RemoveRange(transitiveClosures);
        await appDbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Adds a new transitive closure row.
    /// </summary>
    /// <param name="transitiveClosure">The closure row to add.</param>
    public async Task AddAsync(TransitiveClosure transitiveClosure)
    {
        await appDbContext.TransitiveClosures.AddAsync(transitiveClosure);
        await appDbContext.SaveChangesAsync();
    }
}
