using FluentAssertions;
using TransitiveClosureTable.Domain.Entities;

namespace TransitiveClosureTable.Tests.DomainTests
{
    /// <summary>
    /// Contains unit tests for the <see cref="Tree"/> domain entity.
    /// </summary>
    public class TreeTests
    {
        /// <summary>
        /// Verifies that a new <see cref="Tree"/> initializes its collections correctly.
        /// </summary>
        [Fact]
        public void Tree_Should_Initialize_Collections()
        {
            // Arrange
            var tree = new Tree { Name = "MyTree" };

            // Act & Assert
            tree.Nodes.Should().NotBeNull();
            tree.TransitiveClosures.Should().NotBeNull();
            tree.Nodes.Should().BeEmpty();
            tree.TransitiveClosures.Should().BeEmpty();
        }

        /// <summary>
        /// Verifies that the <see cref="Tree.Name"/> property is correctly assigned during initialization.
        /// </summary>
        [Fact]
        public void Tree_Name_Should_Be_Assigned()
        {
            // Arrange
            var tree = new Tree { Name = "TestTree" };

            // Act & Assert
            tree.Name.Should().Be("TestTree");
        }
    }
}