using TransitiveClosureTable.Application.Services.Contracts;
using TransitiveClosureTable.Domain.Entities;
using TransitiveClosureTable.Domain.Exceptions;
using TransitiveClosureTable.Infrastructure.Factories.Contracts;

namespace TransitiveClosureTable.Application.Services;

public class NodeService(IUnitOfWorkFactory unitOfWorkFactory) : INodeService
{
    public async Task DeleteAsync(int id)
    {
        using var unitOfWork = unitOfWorkFactory.Create();

        var node = await unitOfWork.Nodes.GetByIdAsync(id);

        // check if any have
        // var children = await unitOfWork.Nodes.GetChildrenAsync(id);
        // if (children.Count > 0)
        //    throw new SecureException("You have to delete all children nodes first");

        await unitOfWork.Nodes.DeleteAsync(node ?? throw new SecureException($"Node with id '{id}' not found."));
    }

    public Task<Node> RenameAsync(Node node)
    {
        throw new NotImplementedException();
    }
}