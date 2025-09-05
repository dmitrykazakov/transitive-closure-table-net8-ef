using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TransitiveClosureTable.Domain.Entities;
using TransitiveClosureTable.Infrastructure.Data;
using TransitiveClosureTable.Infrastructure.Data.Repositories;
using TransitiveClosureTable.Infrastructure.Factories;

// assuming UnitOfWork is here

namespace TransitiveClosureTable.Tests.InfrastructureTests;

/// <summary>
///     Contains unit tests for <see cref="TransitiveClosureRepository" /> using an in-memory database
///     and Unit of Work pattern.
/// </summary>
public class TransitiveClosureRepositoryTests
{
    /// <summary>
    ///     Creates a new <see cref="AppDbContext" /> configured with a unique in-memory database.
    ///     Each test uses its own database instance to prevent conflicts.
    /// </summary>
    /// <returns>A new <see cref="AppDbContext" /> instance.</returns>
    private static AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    /// <summary>
    ///     Verifies that <see cref="TransitiveClosureRepository.AddAsync(TransitiveClosure)" /> adds a new closure correctly.
    ///     After adding and committing via UnitOfWork, the total count should be 1.
    /// </summary>
    [Fact]
    public async Task AddAsync_Should_Add_Closure()
    {
        await using var context = GetDbContext();
        await using var unitOfWork = new UnitOfWork(context);
        var transitiveClosureRepository = unitOfWork.TransitiveClosures;

        var closure = new TransitiveClosure
        {
            TreeId = 1,
            AncestorId = 1,
            DescendantId = 1,
            Depth = 0
        };

        await transitiveClosureRepository.AddAsync(closure);
        await unitOfWork.CommitAsync(); // persist changes

        (await context.TransitiveClosures.CountAsync()).Should().Be(1);
    }

    /// <summary>
    ///     Verifies that <see cref="TransitiveClosureRepository.RemoveRangeAsync(IEnumerable{TransitiveClosure})" />
    ///     removes a closure correctly. After deletion and commit, the total count should be 0.
    /// </summary>
    [Fact]
    public async Task DeleteAsync_Should_Remove_Closure()
    {
        await using var context = GetDbContext();
        await using var unitOfWork = new UnitOfWork(context);
        var transitiveClosureRepository = unitOfWork.TransitiveClosures;

        var closure = new TransitiveClosure { TreeId = 1, AncestorId = 1, DescendantId = 1, Depth = 0 };
        await transitiveClosureRepository.AddAsync(closure);
        await unitOfWork.CommitAsync(); // persist

        // Use array instead of invalid [closure]
        await transitiveClosureRepository.RemoveRangeAsync([closure]);
        await unitOfWork.CommitAsync(); // commit deletion

        (await context.TransitiveClosures.CountAsync()).Should().Be(0);
    }
}