using TransitiveClosureTable.Domain.Entities;

namespace TransitiveClosureTable.Infrastructure.Data.Repositories.Contracts;

public interface INodeRepository
{
    Task<Node> GetByIdAsync(int id);
    Task<Node> GetChildrenAsync(int id);
    Task DeleteAsync(Node node);
}