using TransitiveClosureTable.Domain.Entities;

namespace TransitiveClosureTable.Application.Services.Contracts;

public interface INodeService
{
    Task DeleteAsync(int id);
    Task<Node> RenameAsync(Node node);
}