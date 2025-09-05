using FluentAssertions;
using TransitiveClosureTable.Domain.Entities;

namespace TransitiveClosureTable.Tests.DomainTests;

/// <summary>
///     Contains unit tests for the <see cref="Node" /> domain entity.
/// </summary>
public class NodeTests
{
    /// <summary>
    ///     Verifies that a newly created <see cref="Node" /> initializes the <see cref="Node.Ancestors" /> collection
    ///     correctly.
    /// </summary>
    [Fact]
    public void Node_Should_Initialize_Ancestors()
    {
        // Arrange & Act
        var node = new Node { Name = "Node1", TreeId = 1 };

        // Assert
        node.Ancestors.Should().NotBeNull();
        node.Ancestors.Should().BeEmpty();
    }

    /// <summary>
    ///     Verifies that the <see cref="Node.Name" /> and <see cref="Node.TreeId" /> properties are assigned correctly.
    /// </summary>
    [Fact]
    public void Node_Name_And_TreeId_Should_Be_Assigned()
    {
        // Arrange & Act
        var node = new Node { Name = "Node1", TreeId = 42 };

        // Assert
        node.Name.Should().Be("Node1");
        node.TreeId.Should().Be(42);
    }
}