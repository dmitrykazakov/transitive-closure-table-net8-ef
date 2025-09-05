using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TransitiveClosureTable.Domain.Entities;
using TransitiveClosureTable.Infrastructure.Data;
using TransitiveClosureTable.Infrastructure.Data.Repositories;
using TransitiveClosureTable.Infrastructure.Factories;

namespace TransitiveClosureTable.Tests.InfrastructureTests;

/// <summary>
///     Contains unit tests for <see cref="TreeRepository" /> using an in-memory database and UnitOfWork.
/// </summary>
public class TreeRepositoryTests
{
    /// <summary>
    ///     Creates a new <see cref="AppDbContext" /> configured with a unique in-memory database.
    ///     Each test should use its own context to avoid data collisions.
    /// </summary>
    /// <returns>A new instance of <see cref="AppDbContext" />.</returns>
    private static AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    /// <summary>
    ///     Verifies that <see cref="TreeRepository.AddAsync(Tree)" /> adds a new tree correctly.
    ///     After adding, the tree count should be 1 and the name should match.
    /// </summary>
    [Fact]
    public async Task AddAsync_Should_Add_Tree()
    {
        var context = GetDbContext();
        await using var unitOfWork = new UnitOfWork(context);

        var tree = new Tree { Name = "Tree1" };
        await unitOfWork.Trees.AddAsync(tree);
        await unitOfWork.CommitAsync();

        (await context.Trees.CountAsync()).Should().Be(1);
        (await context.Trees.FirstAsync()).Name.Should().Be("Tree1");
    }

    /// <summary>
    ///     Tests <see cref="TreeRepository.GetByNameAsync(string)" />.
    ///     Ensures that fetching an existing tree returns the correct entity,
    ///     and fetching a non-existent name returns null.
    /// </summary>
    [Fact]
    public async Task GetByNameAsync_Should_Return_Tree_Or_Null()
    {
        var context = GetDbContext();
        await using var unitOfWork = new UnitOfWork(context);

        var tree = new Tree { Name = "Tree1" };
        await unitOfWork.Trees.AddAsync(tree);
        await unitOfWork.CommitAsync();

        var fetched = await unitOfWork.Trees.GetByNameAsync("Tree1");
        fetched.Should().NotBeNull();
        fetched.Name.Should().Be("Tree1");

        var notFound = await unitOfWork.Trees.GetByNameAsync("Unknown");
        notFound.Should().BeNull();
    }
}