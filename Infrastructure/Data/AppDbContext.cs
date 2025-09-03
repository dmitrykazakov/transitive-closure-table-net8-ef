using Microsoft.EntityFrameworkCore;
using TransitiveClosureTable.Domain.Entities;

namespace TransitiveClosureTable.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Tree> Trees { get; set; }

    public DbSet<Node> Nodes { get; set; }

    public DbSet<TransitiveClosure> TransitiveClosures { get; set; }

    public DbSet<ExceptionJournal> ExceptionJournals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // --- Tree ---
        modelBuilder.Entity<Tree>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Id).HasColumnOrder(0);
            entity.Property(t => t.Name).IsRequired().HasMaxLength(256).HasColumnOrder(1);
            entity.HasIndex(t => t.Name).IsUnique();
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
            // Composite PK
            entity.HasKey(tc => new { tc.AncestorId, tc.DescendantId });
            entity.Property(tc => tc.TreeId).HasColumnOrder(0);
            entity.Property(tc => tc.AncestorId).HasColumnOrder(1);
            entity.Property(tc => tc.DescendantId).HasColumnOrder(2);
            entity.Property(tc => tc.Depth).HasColumnOrder(3).IsRequired();

            // FKs to Node
            entity.HasOne(tc => tc.Ancestor)
                  .WithMany()
                  .HasForeignKey(tc => tc.AncestorId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(tc => tc.Descendant)
                  .WithMany()
                  .HasForeignKey(tc => tc.DescendantId)
                  .OnDelete(DeleteBehavior.Restrict);

            // FK to Tree for tree independence
            entity.HasOne(tc => tc.Tree)
                  .WithMany(t => t.TransitiveClosures)
                  .HasForeignKey(tc => tc.TreeId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Indexes for performance
            entity.HasIndex(tc => tc.AncestorId);
            entity.HasIndex(tc => tc.DescendantId);
            entity.HasIndex(tc => tc.TreeId);
        });
    }

}