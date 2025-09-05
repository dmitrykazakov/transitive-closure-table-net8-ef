using TransitiveClosureTable.Application.Dto;
using TransitiveClosureTable.Application.Services.Contracts;
using TransitiveClosureTable.Domain.Entities;
using TransitiveClosureTable.Infrastructure.Factories.Contracts;

namespace TransitiveClosureTable.Application.Services;

/// <summary>
///     Service for managing Tree entities and their node hierarchy.
/// </summary>
public class TreeService(IUnitOfWorkFactory unitOfWorkFactory) : ITreeService
{
    /// <summary>
    ///     Gets an existing tree by name or creates a new one if it doesn't exist.
    ///     Ensures a root node and transitive closure entry exists.
    /// </summary>
    /// <param name="name">The name of the tree.</param>
    /// <returns>A TreeDto representing the tree and its nested nodes.</returns>
    public async Task<TreeDto> GetOrCreateAsync(string name)
    {
        using var unitOfWork = unitOfWorkFactory.Create();
        await using var transaction = await unitOfWork.BeginTransactionAsync();

        // Try to load the tree by name
        var tree = await unitOfWork.Trees.GetByNameAsync(name);

        // Create the tree and root node if it doesn't exist
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

            // Add closure row for root node (self-reference)
            await unitOfWork.TransitiveClosures.AddAsync(new TransitiveClosure
            {
                TreeId = tree.Id,
                AncestorId = rootNode.Id,
                DescendantId = rootNode.Id,
                Depth = 0
            });

            await unitOfWork.CommitAsync();
        }

        // Load all nodes and closures for this tree
        var nodes = await unitOfWork.Nodes.GetByTreeIdAsync(tree.Id);
        var closures = await unitOfWork.TransitiveClosures.GetByTreeIdAsync(tree.Id);

        // Build the hierarchical structure
        var rootNodes = BuildNestedTree(nodes, closures);

        await transaction.CommitAsync();

        // We assume only 1 root node per tree
        var root = rootNodes.Single();

        return new TreeDto
        {
            Id = tree.Id,
            Name = tree.Name,
            Root = root.ToDto()
        };
    }

    /// <summary>
    ///     Builds a nested tree structure from a flat list of nodes and their transitive closure entries.
    /// </summary>
    /// <param name="nodes">List of nodes in the tree.</param>
    /// <param name="closures">List of transitive closure entries for the tree.</param>
    /// <returns>List of root nodes with their children properly assigned.</returns>
    private static List<Node> BuildNestedTree(List<Node> nodes, List<TransitiveClosure> closures)
    {
        // Create a dictionary for quick lookup
        var nodeDict = nodes.ToDictionary(n => n.Id);

        // Clear any existing ancestor/child references
        foreach (var node in nodes) node.Descendants.Clear();

        // Link nodes to their direct ancestors (Depth = 1)
        foreach (var closure in closures.Where(c => c.Depth == 1))
            if (nodeDict.TryGetValue(closure.AncestorId, out var parent) &&
                nodeDict.TryGetValue(closure.DescendantId, out var child))
                parent.Descendants.Add(child);

        // Identify root nodes (nodes with no Depth=1 ancestors)
        var rootNodes = nodes
            .Where(n => !closures.Any(c => c.DescendantId == n.Id && c.Depth == 1))
            .ToList();

        return rootNodes;
    }
}