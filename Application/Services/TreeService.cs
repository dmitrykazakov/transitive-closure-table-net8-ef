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

        // Load nodes + closures
        var nodes = await unitOfWork.Nodes.GetByTreeIdAsync(tree.Id);
        var closures = await unitOfWork.TransitiveClosures.GetByTreeIdAsync(tree.Id);

        // Build hierarchy
        var rootNodes = BuildNestedTree(nodes, closures);

        await transaction.CommitAsync();

        // we assume only 1 root per tree
        var root = rootNodes.Single();
        return new TreeDto
        {
            Id = tree.Id,
            Name = tree.Name,
            Root = root.ToDto()
        };
    }

    private static List<Node> BuildNestedTree(List<Node> nodes, List<TransitiveClosure> closures)
    {
        var nodeDict = nodes.ToDictionary(n => n.Id);

        foreach (var node in nodes)
        {
            node.Ancestors.Clear();
        }

        foreach (var closure in closures.Where(c => c.Depth == 1))
        {
            if (nodeDict.TryGetValue(closure.AncestorId, out var parent) && nodeDict.TryGetValue(closure.DescendantId, out var child))
            {
                parent.Ancestors.Add(child);
            }
        }

        var rootNodes = nodes
            .Where(n => !closures.Any(c => c.DescendantId == n.Id && c.Depth == 1))
            .ToList();

        return rootNodes;
    }
}
