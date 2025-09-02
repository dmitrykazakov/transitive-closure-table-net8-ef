using Microsoft.EntityFrameworkCore;
using TransitiveClosureTable.Domain.Entities;
using TransitiveClosureTable.Infrastructure.Data.Repositories.Contracts;

namespace TransitiveClosureTable.Infrastructure.Data.Repositories;

public class TreeRepository(AppDbContext appDbContext) : ITreeRepository
{
    public async Task<Tree?> GetByNameAsync(string name)
    {
        return await appDbContext.Trees.SingleOrDefaultAsync(t => t.Name == name);
    }

    public async Task AddAsync(Tree tree)
    {
        await appDbContext.Trees.AddAsync(tree);
    }
}