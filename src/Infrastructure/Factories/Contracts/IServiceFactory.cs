using TransitiveClosureTable.Application.Services.Contracts;

namespace TransitiveClosureTable.Infrastructure.Factories.Contracts;

/// <summary>
///     Factory interface for creating application service instances.
/// </summary>
public interface IServiceFactory
{
    /// <summary>
    ///     Creates an instance of <see cref="INodeService" />.
    /// </summary>
    /// <returns>A new <see cref="INodeService" /> instance.</returns>
    INodeService CreateNodeService();

    /// <summary>
    ///     Creates an instance of <see cref="ITreeService" />.
    /// </summary>
    /// <returns>A new <see cref="ITreeService" /> instance.</returns>
    ITreeService CreateTreeService();

    /// <summary>
    ///     Creates an instance of <see cref="IExceptionJournalService" />.
    /// </summary>
    /// <returns>A new <see cref="IExceptionJournalService" /> instance.</returns>
    IExceptionJournalService CreateExceptionJournalService();
}