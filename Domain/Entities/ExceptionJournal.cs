using System.ComponentModel.DataAnnotations;

namespace TransitiveClosureTable.Domain.Entities;

public class ExceptionJournal
{
    [Key] public long EventId { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [MaxLength(4096)] public required string StackTrace { get; set; }

    [MaxLength(4096)] public required string QueryParams { get; set; }

    [MaxLength(4096)] public required string BodyParams { get; set; }

    [MaxLength(256)] public required string ExceptionType { get; set; }
}