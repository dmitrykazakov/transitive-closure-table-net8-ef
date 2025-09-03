using FluentAssertions;
using TransitiveClosureTable.Domain.Entities;

namespace TransitiveClosureTable.Tests.DomainTests
{
    /// <summary>
    /// Contains unit tests for the <see cref="TransitiveClosure"/> domain entity.
    /// </summary>
    public class TransitiveClosureTests
    {
        /// <summary>
        /// Verifies that a new <see cref="TransitiveClosure"/> assigns its properties correctly.
        /// </summary>
        [Fact]
        public void TransitiveClosure_Should_Assign_Properties()
        {
            // Arrange & Act
            var closure = new TransitiveClosure
            {
                TreeId = 1,
                AncestorId = 10,
                DescendantId = 20,
                Depth = 2
            };

            // Assert
            closure.TreeId.Should().Be(1);
            closure.AncestorId.Should().Be(10);
            closure.DescendantId.Should().Be(20);
            closure.Depth.Should().Be(2);
        }

        /// <summary>
        /// Verifies that the <see cref="TransitiveClosure.Ancestor"/> and <see cref="TransitiveClosure.Descendant"/> properties
        /// are null by default when not assigned.
        /// </summary>
        [Fact]
        public void TransitiveClosure_Ancestor_And_Descendant_Can_Be_Null()
        {
            // Arrange & Act
            var closure = new TransitiveClosure
            {
                TreeId = 1,
                AncestorId = 5,
                DescendantId = 5,
                Depth = 0
            };

            // Assert
            closure.Ancestor.Should().BeNull();
            closure.Descendant.Should().BeNull();
        }
    }
}