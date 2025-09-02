using TransitiveClosureTable.Domain.Entities;
using TransitiveClosureTable.Infrastructure.Data.Repositories.Contracts;

namespace TransitiveClosureTable.Infrastructure.Data.Repositories;

public class TransitiveClosureRepository(AppDbContext context) : ITransitiveClosureRepository
{
    public async Task AddAsync(TransitiveClosure closure)
    {
        await context.TransitiveClosures.AddAsync(closure);
    }
}