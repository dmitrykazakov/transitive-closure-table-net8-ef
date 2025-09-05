namespace TransitiveClosureTable.Infrastructure.Factories.Contracts;

/// <summary>
/// Factory interface to create service factories for the application.
/// </summary>
public interface IAppFactory
{
    /// <summary>
    /// Creates an instance of <see cref="IServiceFactory"/>.
    /// </summary>
    /// <returns>A new service factory instance.</returns>
    IServiceFactory CreateServiceFactory();
}