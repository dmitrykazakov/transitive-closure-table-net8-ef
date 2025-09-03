using Microsoft.EntityFrameworkCore;
using TransitiveClosureTable.Domain.Entities;
using TransitiveClosureTable.Domain.Exceptions;
using TransitiveClosureTable.Infrastructure.Data.Repositories.Contracts;

namespace TransitiveClosureTable.Infrastructure.Data.Repositories;

public class NodeRepository(AppDbContext appDbContext) : INodeRepository
{
    public async Task<Node> GetByIdAsync(int id)
    {
        var node = await appDbContext.Nodes.SingleOrDefaultAsync(n => n.Id == id) ??
            throw new SecureException($"Node with ID = {id} not found.");
        return node;
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

    public async Task<bool> HasDirectDescendantAsync(int nodeId)
    {
        return await appDbContext.TransitiveClosures
            .AnyAsync(tc => tc.AncestorId == nodeId && tc.Depth == 1);
    }

    public async Task<bool> HasDirectAncestorAsync(int nodeId)
    {
        // Returns true if node has at least one direct parent
        return await appDbContext.TransitiveClosures
            .AnyAsync(tc => tc.DescendantId == nodeId && tc.Depth == 1);
    }
}