using TransitiveClosureTable.Domain.Entities;

namespace TransitiveClosureTable.Infrastructure.Data.Repositories.Contracts;

public interface ITransitiveClosureRepository
{
    Task AddAsync(TransitiveClosure closure);
}