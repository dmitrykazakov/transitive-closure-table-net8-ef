using TransitiveClosureTable.Application.Services.Contracts;
using TransitiveClosureTable.Domain.Entities;
using TransitiveClosureTable.Domain.Exceptions;
using TransitiveClosureTable.Infrastructure.Factories.Contracts;

namespace TransitiveClosureTable.Application.Services;

public class NodeService(IUnitOfWorkFactory unitOfWorkFactory) : INodeService
{
    public async Task<Node> CreateAsync(int treeId, string name, int? parentNodeId = null)
    {
        using var unitOfWork = unitOfWorkFactory.Create();
        await using var transaction = await unitOfWork.BeginTransactionAsync();

        // Create the new node
        var node = new Node
        {
            Name = name,
            TreeId = treeId
        };

        await unitOfWork.Nodes.AddAsync(node);
        await unitOfWork.CommitAsync(); // ensure node.Id is generated

        // 1. Add self-reference in closure table
        await unitOfWork.TransitiveClosures.AddAsync(new TransitiveClosure
        {
            TreeId = treeId,
            AncestorId = node.Id,
            DescendantId = node.Id,
            Depth = 0
        });

        // 2. Add ancestor entries if parentNodeId is specified
        if (parentNodeId.HasValue)
        {
            var parentAncestors = await unitOfWork.TransitiveClosures.GetAncestorsAsync(parentNodeId.Value);

            foreach (var ancestor in parentAncestors)
            {
                await unitOfWork.TransitiveClosures.AddAsync(new TransitiveClosure
                {
                    TreeId = treeId,
                    AncestorId = ancestor.AncestorId,
                    DescendantId = node.Id,
                    Depth = ancestor.Depth + 1
                });
            }
        }

        await unitOfWork.CommitAsync();
        await transaction.CommitAsync();

        return node;
    }

    public async Task<Node> DeleteAsync(int id)
    {
        using var unitOfWork = unitOfWorkFactory.Create();

        var node = await unitOfWork.Nodes.GetByIdAsync(id);

        //var ancestors = await unitOfWork.Nodes.GetAncestorsAsync(id);
        //if (ancestors is { ancestors.Count: > 1 })
        //    throw new SecureException("You have to delete all ancestors nodes first");

        await unitOfWork.Nodes.DeleteAsync(node ?? throw new SecureException($"Node with id '{id}' not found."));

        return node;
    }

    public Task<Node> RenameAsync(Node node)
    {
        throw new NotImplementedException();
    }
}
