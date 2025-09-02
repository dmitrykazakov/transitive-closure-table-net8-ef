using TransitiveClosureTable.Domain.Entities;

namespace TransitiveClosureTable.Infrastructure.Data.Repositories.Contracts;

public interface ITreeRepository
{
    Task<Tree?> GetByNameAsync(string name);
    Task AddAsync(Tree tree);
}