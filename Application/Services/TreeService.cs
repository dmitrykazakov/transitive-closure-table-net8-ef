using TransitiveClosureTable.Application.Dto;
using TransitiveClosureTable.Application.Services.Contracts;
using TransitiveClosureTable.Domain.Entities;
using TransitiveClosureTable.Infrastructure.Factories.Contracts;

namespace TransitiveClosureTable.Application.Services;

public class TreeService(IUnitOfWorkFactory unitOfWorkFactory) : ITreeService
{
    public async Task<TreeDto> GetOrCreateAsync(string name)
    {
        using var unitOfWork = unitOfWorkFactory.Create();
        await using var transaction = await unitOfWork.BeginTransactionAsync();

        var tree = await unitOfWork.Trees.GetByNameAsync(name);

        if (tree == null)
        {
            tree = new Tree { Name = name };

            await unitOfWork.Trees.AddAsync(tree);
            await unitOfWork.CommitAsync();

            // Create root node
            var rootNode = new Node
            {
                Name = "Root",
                TreeId = tree.Id
            };

            await unitOfWork.Nodes.AddAsync(rootNode);
            await unitOfWork.CommitAsync();

            // Add closure row for root
            await unitOfWork.TransitiveClosures.AddAsync(new TransitiveClosure
            {
                TreeId = tree.Id,
                AncestorId = rootNode.Id,
                DescendantId = rootNode.Id,
                Depth = 0
            });

            await unitOfWork.CommitAsync();
        }

        await transaction.CommitAsync();

        return tree.ToDto();
    }

    private static List<Node> BuildNestedTree(List<Node> nodes, List<TransitiveClosure> closures)
    {
        var nodeDict = nodes.ToDictionary(n => n.Id);

        foreach (var node in nodes)
        {
            node.Children.Clear();
        }

        foreach (var closure in closures.Where(c => c.Depth == 1))
        {
            if (nodeDict.TryGetValue(closure.AncestorId, out var parent) && nodeDict.TryGetValue(closure.DescendantId, out var child))
            {
                parent.Children.Add(child);
            }
        }

        var rootNodes = nodes
            .Where(n => !closures.Any(c => c.DescendantId == n.Id && c.Depth == 1))
            .ToList();

        return rootNodes;
    }
}
