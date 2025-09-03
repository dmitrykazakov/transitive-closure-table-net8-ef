using Microsoft.EntityFrameworkCore;
using TransitiveClosureTable.Domain.Entities;
using TransitiveClosureTable.Infrastructure.Data.Repositories.Contracts;

namespace TransitiveClosureTable.Infrastructure.Data.Repositories;

public class TransitiveClosureRepository(AppDbContext appDbContext) : ITransitiveClosureRepository
{
    public async Task<List<TransitiveClosure>> GetAncestorsAsync(int descendantId)
    {
        return await appDbContext.TransitiveClosures
            .Where(tc => tc.DescendantId == descendantId)
            .OrderBy(tc => tc.Depth) // optional: closest ancestor first
            .ToListAsync();
    }

    public async Task AddAsync(TransitiveClosure transitiveClosure)
    {
        await appDbContext.TransitiveClosures.AddAsync(transitiveClosure);
        await appDbContext.SaveChangesAsync();
    }
}