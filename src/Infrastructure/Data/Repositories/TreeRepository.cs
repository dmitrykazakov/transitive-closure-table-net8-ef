using Microsoft.EntityFrameworkCore;
using TransitiveClosureTable.Domain.Entities;
using TransitiveClosureTable.Infrastructure.Data.Repositories.Contracts;

namespace TransitiveClosureTable.Infrastructure.Data.Repositories;

/// <summary>
///     Repository implementation for performing CRUD operations on <see cref="Tree" /> entities.
/// </summary>
public class TreeRepository(AppDbContext appDbContext) : ITreeRepository
{
    /// <summary>
    ///     Retrieves a tree by its name.
    /// </summary>
    /// <param name="name">The name of the tree to retrieve.</param>
    /// <returns>The <see cref="Tree" /> with the specified name, or null if not found.</returns>
    public async Task<Tree?> GetByNameAsync(string name)
    {
        return await appDbContext.Trees.SingleOrDefaultAsync(t => t.Name == name);
    }

    /// <summary>
    ///     Adds a new tree to the database.
    /// </summary>
    /// <param name="tree">The <see cref="Tree" /> entity to add.</param>
    public async Task AddAsync(Tree tree)
    {
        await appDbContext.Trees.AddAsync(tree);
    }
}