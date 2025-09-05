using System.ComponentModel;

namespace TransitiveClosureTable.Application.Dto;

/// <summary>
///     Request DTO for filtering and paginating exception journal entries.
/// </summary>
public class GetExceptionJournalRequestRangeDto
{
    /// <summary>
    ///     Number of items to skip (used for pagination).
    ///     Default is <c>0</c>.
    /// </summary>
    [DefaultValue(0)]
    public int Skip { get; set; }

    /// <summary>
    ///     Maximum number of items to return (used for pagination).
    ///     Default is <c>50</c>.
    /// </summary>
    [DefaultValue(50)]
    public int Take { get; set; }

    /// <summary>
    ///     Optional filter: lower bound of the timestamp range (UTC).
    ///     Returns only entries created at or after this time.
    /// </summary>
    public DateTime? FromTimestamp { get; set; }

    /// <summary>
    ///     Optional filter: upper bound of the timestamp range (UTC).
    ///     Returns only entries created at or before this time.
    /// </summary>
    public DateTime? ToTimestamp { get; set; }

    /// <summary>
    ///     Optional filter: exception type name (e.g., <c>InvalidOperationException</c>).
    /// </summary>
    [DefaultValue("")]
    public string? ExceptionType { get; set; }

    /// <summary>
    ///     Optional filter: substring that must be contained in the query string parameters.
    /// </summary>
    [DefaultValue("")]
    public string? QueryContains { get; set; }

    /// <summary>
    ///     Optional filter: substring that must be contained in the body parameters.
    /// </summary>
    [DefaultValue("")]
    public string? BodyContains { get; set; }
}