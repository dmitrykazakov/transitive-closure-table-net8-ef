using TransitiveClosureTable.Domain.Entities;

namespace TransitiveClosureTable.Infrastructure.Data.Repositories.Contracts;

public interface INodeRepository
{
    Task<Node> GetByIdAsync(int id);
    Task DeleteAsync(Node node);
    Task AddAsync(Node rootNode);
    Task<List<Node>> GetChildrenAsync(int parentId);
}