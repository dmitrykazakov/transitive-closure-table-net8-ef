using Microsoft.EntityFrameworkCore;
using TransitiveClosureTable.Domain.Entities;
using TransitiveClosureTable.Infrastructure.Data.Repositories.Contracts;

namespace TransitiveClosureTable.Infrastructure.Data.Repositories;

public class NodeRepository(AppDbContext appDbContext) : INodeRepository
{
    public async Task<Node> GetByIdAsync(int id)
    {
        return await appDbContext.Nodes.SingleAsync(n => n.Id == id);
    }
    public async Task DeleteAsync(Node node)
    {
        appDbContext.Nodes.Remove(node);
        await appDbContext.SaveChangesAsync();
    }

    public async Task AddAsync(Node node)
    {
        await appDbContext.Nodes.AddAsync(node);
        await appDbContext.SaveChangesAsync();
    }

    public async Task<List<Node>> GetByTreeIdAsync(int treeId)
    {
        return await appDbContext.Nodes
            .Where(n => n.TreeId == treeId)
            .ToListAsync();
    }
}