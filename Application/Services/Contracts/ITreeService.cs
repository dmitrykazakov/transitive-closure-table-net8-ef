using TransitiveClosureTable.Domain.Entities;

namespace TransitiveClosureTable.Application.Services.Contracts;

public interface ITreeService
{
    Task<Tree> GetOrCreateAsync(string name);
}