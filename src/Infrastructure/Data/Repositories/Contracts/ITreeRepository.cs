using TransitiveClosureTable.Domain.Entities;

namespace TransitiveClosureTable.Infrastructure.Data.Repositories.Contracts;

/// <summary>
///     Repository interface for performing CRUD operations on <see cref="Tree" /> entities.
/// </summary>
public interface ITreeRepository
{
    /// <summary>
    ///     Retrieves a tree by its name.
    /// </summary>
    /// <param name="name">The name of the tree.</param>
    /// <returns>The <see cref="Tree" /> with the specified name, or null if not found.</returns>
    Task<Tree?> GetByNameAsync(string name);

    /// <summary>
    ///     Adds a new tree to the database.
    /// </summary>
    /// <param name="tree">The <see cref="Tree" /> entity to add.</param>
    Task AddAsync(Tree tree);
}