namespace TransitiveClosureTable.Application.Dto;

public class NodeDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<NodeDto> Ancestors { get; set; } = [];
}
