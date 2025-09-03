using Microsoft.EntityFrameworkCore;
using TransitiveClosureTable.Domain.Entities;
using TransitiveClosureTable.Infrastructure.Data.Repositories.Contracts;

namespace TransitiveClosureTable.Infrastructure.Data.Repositories;

public class TransitiveClosureRepository(AppDbContext appDbContext) : ITransitiveClosureRepository
{
    // Get all ancestors of a node
    public async Task<List<TransitiveClosure>> GetAncestorsAsync(int descendantId)
    {
        return await appDbContext.TransitiveClosures
            .Where(tc => tc.DescendantId == descendantId)
            .OrderBy(tc => tc.Depth) // optional: closest ancestor first
            .ToListAsync();
    }

    // Get all closures for a given tree
    public async Task<List<TransitiveClosure>> GetByTreeIdAsync(int treeId)
    {
        return await appDbContext.TransitiveClosures
            .Where(tc => tc.TreeId == treeId)
            .ToListAsync();
    }

    // Delete a single closure row
    public async Task DeleteAsync(TransitiveClosure? closure)
    {
        if (closure == null)
            return;

        appDbContext.TransitiveClosures.Remove(closure);
        await appDbContext.SaveChangesAsync();
    }

    public async Task<List<TransitiveClosure>> GetAllByNodeIdAsync(int nodeId)
    {
        return await appDbContext.TransitiveClosures
            .Where(tc => tc.DescendantId == nodeId || tc.AncestorId == nodeId)
            .ToListAsync();
    }

    // Add a closure row
    public async Task AddAsync(TransitiveClosure transitiveClosure)
    {
        await appDbContext.TransitiveClosures.AddAsync(transitiveClosure);
        await appDbContext.SaveChangesAsync();
    }
}