namespace TransitiveClosureTable.Domain.Exceptions;

/// <summary>
///     Exception type used to signal security or business logic violations
///     such as forbidden operations on tree nodes (e.g., deleting a root node).
/// </summary>
public class SecureException : Exception
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="SecureException" /> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public SecureException(string message) : base(message)
    {
    }
}