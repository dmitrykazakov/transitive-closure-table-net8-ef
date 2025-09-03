using TransitiveClosureTable.Domain.Entities;

namespace TransitiveClosureTable.Infrastructure.Data.Repositories.Contracts;

public interface INodeRepository
{
    Task<Node> GetByIdAsync(int nodeId);
    Task DeleteAsync(Node node);
    Task AddAsync(Node node);
    Task<List<Node>> GetByTreeIdAsync(int treeId);
    Task<bool> HasDirectDescendantAsync(int nodeId);
    Task<bool> HasDirectAncestorAsync(int id);
}