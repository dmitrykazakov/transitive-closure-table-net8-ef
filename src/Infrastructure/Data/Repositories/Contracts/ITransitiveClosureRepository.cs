using TransitiveClosureTable.Domain.Entities;

namespace TransitiveClosureTable.Infrastructure.Data.Repositories.Contracts;

/// <summary>
/// Repository interface for managing TransitiveClosure entities.
/// </summary>
public interface ITransitiveClosureRepository
{
    /// <summary>
    /// Adds a new transitive closure row.
    /// </summary>
    /// <param name="transitiveClosure">The closure row to add.</param>
    Task AddAsync(TransitiveClosure transitiveClosure);

    /// <summary>
    /// Gets all ancestor closures of a specific node.
    /// </summary>
    /// <param name="parentNodeId">The node ID for which to find ancestor closures.</param>
    /// <returns>List of TransitiveClosure entries representing ancestors.</returns>
    Task<List<TransitiveClosure>> GetAncestorsAsync(int parentNodeId);

    /// <summary>
    /// Gets all transitive closure rows for a specific tree.
    /// </summary>
    /// <param name="treeId">The tree ID.</param>
    /// <returns>List of TransitiveClosure entries belonging to the tree.</returns>
    Task<List<TransitiveClosure>> GetByTreeIdAsync(int treeId);

    /// <summary>
    /// Deletes a single transitive closure row.
    /// </summary>
    /// <param name="closure">The closure row to delete.</param>
    Task DeleteAsync(TransitiveClosure closure);

    /// <summary>
    /// Gets all closure rows that reference a specific node, either as ancestor or descendant.
    /// </summary>
    /// <param name="nodeId">The node ID.</param>
    /// <returns>List of related TransitiveClosure entries.</returns>
    Task<List<TransitiveClosure>> GetAllByNodeIdAsync(int nodeId);

    /// <summary>
    /// Deletes multiple transitive closure rows at once.
    /// </summary>
    /// <param name="transitiveClosures">Collection of closures to delete.</param>
    Task RemoveRangeAsync(IEnumerable<TransitiveClosure> transitiveClosures);
}