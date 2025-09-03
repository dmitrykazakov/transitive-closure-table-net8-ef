using TransitiveClosureTable.Domain.Entities;

namespace TransitiveClosureTable.Application.Services.Contracts;

public interface INodeService
{
    Task<Node> DeleteAsync(int id);
    Task<Node> RenameAsync(Node node);
    Task<Node> CreateAsync(int treeId, string name, int? parentNodeId = null);
}