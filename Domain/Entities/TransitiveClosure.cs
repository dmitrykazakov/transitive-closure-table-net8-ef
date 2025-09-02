namespace TransitiveClosureTable.Domain.Entities;

public class TransitiveClosure
{
    public long AncestorId { get; set; }

    public long DescendantId { get; set; }

    public int Depth { get; set; }

    public Tree? Ancestor { get; set; }

    public Tree? Descendant { get; set; }
}