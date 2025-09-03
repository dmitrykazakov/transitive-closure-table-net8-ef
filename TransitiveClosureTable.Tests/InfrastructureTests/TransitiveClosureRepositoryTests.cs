using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TransitiveClosureTable.Domain.Entities;
using TransitiveClosureTable.Infrastructure.Data;
using TransitiveClosureTable.Infrastructure.Data.Repositories;

namespace TransitiveClosureTable.Tests.InfrastructureTests
{
    /// <summary>
    /// Contains unit tests for <see cref="TransitiveClosureRepository"/> using an in-memory database.
    /// </summary>
    public class TransitiveClosureRepositoryTests
    {
        /// <summary>
        /// Creates a new <see cref="AppDbContext"/> configured with a unique in-memory database.
        /// Each test uses its own database instance to prevent conflicts.
        /// </summary>
        /// <returns>A new <see cref="AppDbContext"/> instance.</returns>
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        /// <summary>
        /// Verifies that <see cref="TransitiveClosureRepository.AddAsync(TransitiveClosure)"/> adds a new closure correctly.
        /// After adding, the total count should be 1.
        /// </summary>
        [Fact]
        public async Task AddAsync_Should_Add_Closure()
        {
            var context = GetDbContext();
            var repo = new TransitiveClosureRepository(context);

            var closure = new TransitiveClosure
            {
                TreeId = 1,
                AncestorId = 1,
                DescendantId = 1,
                Depth = 0
            };

            await repo.AddAsync(closure);
            (await context.TransitiveClosures.CountAsync()).Should().Be(1);
        }

        /// <summary>
        /// Tests <see cref="TransitiveClosureRepository.GetAncestorsAsync(int)"/>.
        /// Ensures that fetching ancestors returns the correct ancestor list for a given descendant.
        /// </summary>
        [Fact]
        public async Task GetAncestorsAsync_Should_Return_Ancestors()
        {
            var context = GetDbContext();
            var repo = new TransitiveClosureRepository(context);

            await repo.AddAsync(new TransitiveClosure { TreeId = 1, AncestorId = 1, DescendantId = 2, Depth = 1 });
            await repo.AddAsync(new TransitiveClosure { TreeId = 1, AncestorId = 2, DescendantId = 3, Depth = 1 });

            var ancestors = await repo.GetAncestorsAsync(3);
            ancestors.Should().HaveCount(1);
            ancestors.First().AncestorId.Should().Be(2);
        }

        /// <summary>
        /// Verifies that <see cref="TransitiveClosureRepository.DeleteAsync(TransitiveClosure)"/> removes a closure correctly.
        /// After deletion, the total count should be 0.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_Should_Remove_Closure()
        {
            var context = GetDbContext();
            var repo = new TransitiveClosureRepository(context);

            var closure = new TransitiveClosure { TreeId = 1, AncestorId = 1, DescendantId = 1, Depth = 0 };
            await repo.AddAsync(closure);

            await repo.DeleteAsync(closure);
            (await context.TransitiveClosures.CountAsync()).Should().Be(0);
        }
    }
}
