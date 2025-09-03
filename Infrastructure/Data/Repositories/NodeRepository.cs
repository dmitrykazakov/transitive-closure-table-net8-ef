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

    // Get all direct children of a node
    public async Task<List<Node>> GetChildrenAsync(int parentId)
    {
        // Assuming Nodes have a ParentId column, if not use closure table
        return await appDbContext.Nodes
            .Where(n => appDbContext.TransitiveClosures
                .Any(tc => tc.AncestorId == parentId && tc.DescendantId == n.Id && tc.Depth == 1))
            .ToListAsync();
    }
}