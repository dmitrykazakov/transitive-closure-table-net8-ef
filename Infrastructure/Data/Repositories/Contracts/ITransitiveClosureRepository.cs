using System.Collections;
using TransitiveClosureTable.Domain.Entities;

namespace TransitiveClosureTable.Infrastructure.Data.Repositories.Contracts;

public interface ITransitiveClosureRepository
{
    Task AddAsync(TransitiveClosure transitiveClosure);
    Task<List<TransitiveClosure>> GetAncestorsAsync(int parentNodeId);
}