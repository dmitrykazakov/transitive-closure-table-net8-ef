using Microsoft.EntityFrameworkCore;
using TransitiveClosureTable.Domain.Entities;
using TransitiveClosureTable.Infrastructure.Data.Repositories.Contracts;

namespace TransitiveClosureTable.Infrastructure.Data.Repositories;

/// <summary>
///     Repository for managing TransitiveClosure entities.
/// </summary>
public class TransitiveClosureRepository(AppDbContext appDbContext) : ITransitiveClosureRepository
{
    /// <summary>
    ///     Gets all transitive closure rows for a specific tree.
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
    ///     Gets all closure rows that reference a specific node, either as ancestor or descendant.
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
    ///     Deletes multiple transitive closure rows at once.
    /// </summary>
    /// <param name="transitiveClosures">Collection of closures to delete.</param>
    public async Task RemoveRangeAsync(IEnumerable<TransitiveClosure>? transitiveClosures)
    {
        if (transitiveClosures == null) return;

        // Convert to array once to avoid multiple enumeration
        var closures = transitiveClosures as TransitiveClosure[] ?? transitiveClosures.ToArray();

        // Null out navigation properties
        foreach (var closure in closures)
        {
            closure.Ancestor = null;
            closure.Descendant = null;
        }

        // Remove from DbContext
        appDbContext.TransitiveClosures.RemoveRange(closures);

        // Persist changes
        await appDbContext.SaveChangesAsync();
    }


    /// <summary>
    ///     Adds transitive closure records for a new node.
    ///     Inserts:
    ///     1) Self-reference (Depth = 0).
    ///     2) All ancestors of parent + new descendant with Depth + 1.
    /// </summary>
    /// <param name="nodeId">The new node's ID (descendant).</param>
    /// <param name="parentNodeId">The parent node's ID. If null, node is root.</param>
    public async Task AddAsync(int nodeId, int parentNodeId)
    {
        // Get parent's self-reference to extract TreeId
        var parentClosure = await appDbContext.TransitiveClosures
            .SingleAsync(tc => tc.AncestorId == parentNodeId && tc.DescendantId == parentNodeId);

        var treeId = parentClosure.TreeId;

        // Get all ancestors of parent
        var parentAncestors = await appDbContext.TransitiveClosures
            .Where(tc => tc.DescendantId == parentNodeId)
            .ToListAsync();

        // Build ancestor → child closures in one pass
        var newClosures = parentAncestors.Select(ancestor => new TransitiveClosure
        {
            TreeId = treeId,
            AncestorId = ancestor.AncestorId,
            DescendantId = nodeId,
            Depth = ancestor.Depth + 1
        }).ToList();

        await appDbContext.TransitiveClosures.AddRangeAsync(newClosures);


        // Add self-reference
        var selfClosure = new TransitiveClosure
        {
            TreeId = treeId,
            AncestorId = nodeId,
            DescendantId = nodeId,
            Depth = 0
        };

        await appDbContext.TransitiveClosures.AddAsync(selfClosure);
    }

    /// <summary>
    ///     Adds a new transitive closure row.
    /// </summary>
    /// <param name="transitiveClosure">The closure row to add.</param>
    public async Task AddAsync(TransitiveClosure transitiveClosure)
    {
        await appDbContext.TransitiveClosures.AddAsync(transitiveClosure);
    }

    /// <summary>
    ///     Gets all ancestor closures of a given node.
    /// </summary>
    /// <param name="nodeId">The node ID for which to find ancestors.</param>
    /// <returns>List of TransitiveClosure entries representing ancestors.</returns>
    public async Task<List<TransitiveClosure>> GetAncestorsAsync(int nodeId)
    {
        return await appDbContext.TransitiveClosures
            .Where(tc => tc.DescendantId == nodeId)
            .OrderBy(tc => tc.Depth) // optional: closest ancestor first
            .ToListAsync();
    }

    /// <summary>
    ///     Deletes a single transitive transitiveClosure row.
    /// </summary>
    /// <param name="transitiveClosure">The transitiveClosure row to delete. If null, nothing happens.</param>
    public async Task DeleteAsync(TransitiveClosure? transitiveClosure)
    {
        if (transitiveClosure == null)
            return;

        appDbContext.TransitiveClosures.Remove(transitiveClosure);

        await Task.CompletedTask;
    }
}