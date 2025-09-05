namespace TransitiveClosureTable.Application.Dto;

/// <summary>
///     Request DTO for retrieving a single exception journal entry by its ID.
/// </summary>
public class GetSingleExceptionJournalRequestDto
{
    /// <summary>
    ///     The unique identifier of the exception journal entry to retrieve.
    /// </summary>
    public long EventId { get; set; }
}
