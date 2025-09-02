using System.ComponentModel.DataAnnotations;

namespace TransitiveClosureTable.Domain.Entities;

public class Node
{
    [Key] public required long Id { get; set; }

    [MaxLength(100)] public required long TreeId { get; set; }

    public required Tree Tree { get; set; }
}