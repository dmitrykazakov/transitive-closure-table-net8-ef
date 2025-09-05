using System.ComponentModel.DataAnnotations;

namespace TransitiveClosureTable.Domain.Entities;

/// <summary>
/// Entity representing a logged exception, including stack trace and HTTP request information.
/// </summary>
public class ExceptionJournal
{
    /// <summary>
    /// Primary key of the exception event.
    /// </summary>
    [Key]
    public long EventId { get; set; }

    /// <summary>
    /// Timestamp when the exception was logged (UTC).
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Full stack trace of the exception.
    /// </summary>
    [MaxLength(4096)]
    public required string StackTrace { get; set; }

    /// <summary>
    /// Query string parameters of the HTTP request that caused the exception.
    /// </summary>
    [MaxLength(4096)]
    public required string QueryParams { get; set; }

    /// <summary>
    /// Body parameters of the HTTP request that caused the exception.
    /// </summary>
    [MaxLength(4096)]
    public required string BodyParams { get; set; }

    /// <summary>
    /// Type name of the exception (e.g., InvalidOperationException).
    /// </summary>
    [MaxLength(256)]
    public required string ExceptionType { get; set; }
}