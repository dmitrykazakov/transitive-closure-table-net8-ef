namespace TransitiveClosureTable.Application.Dto;

public class CreateNodeRequestDto
{
    public required int TreeId { get; set; }
    public required string Name { get; set; }
    public required int ParentNodeId { get; set; }
}
