using TransitiveClosureTable.Infrastructure.Data;
using TransitiveClosureTable.Infrastructure.Factories.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace TransitiveClosureTable.Infrastructure.Factories;

/// <summary>
/// Factory to create instances of <see cref="IUnitOfWork"/> with a scoped DbContext.
/// </summary>
public class UnitOfWorkFactory(IServiceProvider serviceProvider) : IUnitOfWorkFactory
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    /// <summary>
    /// Creates a new <see cref="IUnitOfWork"/> instance with a scoped DbContext.
    /// </summary>
    /// <returns>An initialized <see cref="IUnitOfWork"/>.</returns>
    public IUnitOfWork Create()
    {
        // Create a new DI scope to manage lifetime of DbContext
        var scope = _serviceProvider.CreateScope();

        // Resolve the AppDbContext from the scoped service provider
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Return a new UnitOfWork instance using the scoped context
        return new UnitOfWork(context);
    }
}