using TransitiveClosureTable.Domain.Entities;

namespace TransitiveClosureTable.Application.Dto;

public class TreeDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<NodeDto> Nodes { get; set; } = [];
}
public static class TreeMappingExtensions
{
    public static TreeDto ToDto(this Tree tree)
    {
        return new TreeDto
        {
            Id = tree.Id,
            Name = tree.Name,
            Nodes = tree.Nodes.Select(n => n.ToDto()).ToList() ?? []
        };
    }

    public static NodeDto ToDto(this Node node)
    {
        return new NodeDto
        {
            Id = node.Id,
            Name = node.Name,
            Children = node.Children.Select(c => c.ToDto()).ToList() ?? []
        };
    }
}
