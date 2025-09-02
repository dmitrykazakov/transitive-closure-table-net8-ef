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

        modelBuilder.Entity<TransitiveClosure>()
            .HasKey(tc => new { tc.AncestorId, tc.DescendantId });

        modelBuilder.Entity<TransitiveClosure>()
            .HasOne(tc => tc.Ancestor)
            .WithMany()
            .HasForeignKey(tc => tc.AncestorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TransitiveClosure>()
            .HasOne(tc => tc.Descendant)
            .WithMany()
            .HasForeignKey(tc => tc.DescendantId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Node>()
            .HasOne(n => n.Tree)
            .WithMany(t => t.Nodes)
            .HasForeignKey(n => n.TreeId);

        modelBuilder.Entity<Tree>()
            .HasIndex(t => t.Name)
            .IsUnique();
    }
}