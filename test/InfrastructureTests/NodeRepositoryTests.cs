using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TransitiveClosureTable.Domain.Entities;
using TransitiveClosureTable.Domain.Exceptions;
using TransitiveClosureTable.Infrastructure.Data;
using TransitiveClosureTable.Infrastructure.Data.Repositories;

namespace TransitiveClosureTable.Tests.InfrastructureTests;

/// <summary>
/// Contains unit tests for <see cref="NodeRepository"/> using an in-memory database.
/// </summary>
public class NodeRepositoryTests
{
    /// <summary>
    /// Creates a new <see cref="AppDbContext"/> configured with a unique in-memory database.
    /// Each test uses its own database instance to prevent conflicts.
    /// </summary>
    /// <returns>A new <see cref="AppDbContext"/> instance.</returns>
    private static AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    /// <summary>
    /// Verifies that <see cref="NodeRepository.AddAsync(Node)"/> adds a new node correctly.
    /// After adding, the total count should be 1 and the node's properties should match.
    /// </summary>
    [Fact]
    public async Task AddAsync_Should_Add_Node()
    {
        var context = GetDbContext();
        var repo = new NodeRepository(context);

        var node = new Node { Name = "Node1", TreeId = 1 };
        await repo.AddAsync(node);

        (await context.Nodes.CountAsync()).Should().Be(1);
        (await context.Nodes.FirstAsync()).Name.Should().Be("Node1");
    }

    /// <summary>
    /// Tests <see cref="NodeRepository.GetByIdAsync(int)"/>.
    /// Ensures fetching an existing node returns the correct entity,
    /// and fetching a non-existing node throws a <see cref="SecureException"/>.
    /// </summary>
    [Fact]
    public async Task GetByIdAsync_Should_Return_Node_Or_Throw()
    {
        var context = GetDbContext();
        var repo = new NodeRepository(context);
        var node = new Node { Name = "Node1", TreeId = 1 };
        await repo.AddAsync(node);

        var fetched = await repo.GetByIdAsync(node.Id);
        fetched.Should().NotBeNull();
        fetched.Name.Should().Be("Node1");

        await repo.Invoking(r => r.GetByIdAsync(999))
            .Should().ThrowAsync<SecureException>();
    }

    /// <summary>
    /// Verifies that <see cref="NodeRepository.DeleteAsync(Node)"/> removes a node correctly.
    /// After deletion, the total count should be 0.
    /// </summary>
    [Fact]
    public async Task DeleteAsync_Should_Remove_Node()
    {
        var context = GetDbContext();
        var repo = new NodeRepository(context);
        var node = new Node { Name = "Node1", TreeId = 1 };
        await repo.AddAsync(node);

        await repo.DeleteAsync(node);
        (await context.Nodes.CountAsync()).Should().Be(0);
    }
}