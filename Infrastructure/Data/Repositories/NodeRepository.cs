using TransitiveClosureTable.Domain.Entities;
using TransitiveClosureTable.Infrastructure.Data.Repositories.Contracts;

namespace TransitiveClosureTable.Infrastructure.Data.Repositories;

public class NodeRepository(AppDbContext appDbContext) : INodeRepository
{
    public Task<Node> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Node> GetChildrenAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Node node)
    {
        throw new NotImplementedException();
    }
}