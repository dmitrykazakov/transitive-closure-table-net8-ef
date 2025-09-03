namespace TransitiveClosureTable.Domain.Entities;

public class TransitiveClosure
{
    public int TreeId { get; set; }      
    public Tree? Tree { get; set; }
    public int AncestorId { get; set; }  
    public Node? Ancestor { get; set; }
    public int DescendantId { get; set; }
    public Node? Descendant { get; set; }
    public int Depth { get; set; } 
}