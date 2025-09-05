using Microsoft.EntityFrameworkCore;
using TransitiveClosureTable.Domain.Entities;

namespace TransitiveClosureTable.Infrastructure.Data;

/// <summary>
///     Entity Framework Core DbContext for the Transitive Closure Table system.
///     Manages Trees, Nodes, TransitiveClosures, and ExceptionJournals.
/// </summary>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    /// <summary>
    ///     DbSet for <see cref="Tree" /> entities.
    /// </summary>
    public DbSet<Tree> Trees { get; set; }

    /// <summary>
    ///     DbSet for <see cref="Node" /> entities.
    /// </summary>
    public DbSet<Node> Nodes { get; set; }

    /// <summary>
    ///     DbSet for <see cref="TransitiveClosure" /> entities.
    /// </summary>
    public DbSet<TransitiveClosure> TransitiveClosures { get; set; }

    /// <summary>
    ///     DbSet for <see cref="ExceptionJournal" /> entities.
    /// </summary>
    public DbSet<ExceptionJournal> ExceptionJournals { get; set; }

    /// <summary>
    ///     Configures the EF Core model: primary keys, relationships, constraints, and indexes.
    /// </summary>
    /// <param name="modelBuilder">The EF Core model builder.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // --- Tree ---
        modelBuilder.Entity<Tree>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Name).IsRequired().HasColumnOrder(1);
            entity.HasIndex(t => t.Name).IsUnique();
            entity.HasMany(t => t.Nodes)
                .WithOne(n => n.Tree)
                .HasForeignKey(n => n.TreeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(t => t.TransitiveClosures)
                .WithOne(tc => tc.Tree)
                .HasForeignKey(tc => tc.TreeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // --- Node ---
        modelBuilder.Entity<Node>(entity =>
        {
            entity.HasKey(n => n.Id);
            entity.Property(n => n.Id).HasColumnOrder(0);
            entity.Property(n => n.TreeId).HasColumnOrder(1);
            entity.Property(n => n.Name).IsRequired().HasMaxLength(256).HasColumnOrder(2);

            entity.HasOne(n => n.Tree)
                .WithMany(t => t.Nodes)
                .HasForeignKey(n => n.TreeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // --- TransitiveClosure ---
        modelBuilder.Entity<TransitiveClosure>(entity =>
        {
            // Composite primary key
            entity.HasKey(tc => new { tc.AncestorId, tc.DescendantId });
            entity.Property(tc => tc.TreeId).HasColumnOrder(0);
            entity.Property(tc => tc.AncestorId).HasColumnOrder(1);
            entity.Property(tc => tc.DescendantId).HasColumnOrder(2);
            entity.Property(tc => tc.Depth).HasColumnOrder(3).IsRequired();

            // Foreign key to Ancestor node
            entity.HasOne(tc => tc.Ancestor)
                .WithMany()
                .HasForeignKey(tc => tc.AncestorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Foreign key to Descendant node
            entity.HasOne(tc => tc.Descendant)
                .WithMany()
                .HasForeignKey(tc => tc.DescendantId)
                .OnDelete(DeleteBehavior.Restrict);

            // Foreign key to Tree for tree isolation
            entity.HasOne(tc => tc.Tree)
                .WithMany(t => t.TransitiveClosures)
                .HasForeignKey(tc => tc.TreeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes for performance
            entity.HasIndex(tc => tc.AncestorId);
            entity.HasIndex(tc => tc.DescendantId);
            entity.HasIndex(tc => tc.TreeId);
        });

        // --- ExceptionJournal ---
        modelBuilder.Entity<ExceptionJournal>(entity =>
        {
            entity.HasKey(e => e.EventId);
            entity.Property(e => e.Timestamp).IsRequired();
            entity.Property(e => e.StackTrace);
            entity.Property(e => e.BodyParams);
            entity.Property(e => e.ExceptionType).IsRequired();
        });
    }
}