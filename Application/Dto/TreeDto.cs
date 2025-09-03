using TransitiveClosureTable.Domain.Entities;

namespace TransitiveClosureTable.Application.Dto;

public class TreeDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required NodeDto Root { get; set; }
}
public static class TreeMappingExtensions
{
    public static NodeDto ToDto(this Node node)
    {
        return new NodeDto
        {
            Id = node.Id,
            Name = node.Name,
            Ancestors = node.Ancestors.Select(c => c.ToDto()).ToList()
        };
    }
}
