namespace TransitiveClosureTable.Infrastructure.Factories.Contracts;

/// <summary>
///     Factory interface for creating <see cref="IUnitOfWork" /> instances.
/// </summary>
public interface IUnitOfWorkFactory
{
    /// <summary>
    ///     Creates a new unit of work instance.
    /// </summary>
    /// <returns>A fresh <see cref="IUnitOfWork" />.</returns>
    IUnitOfWork Create();
}