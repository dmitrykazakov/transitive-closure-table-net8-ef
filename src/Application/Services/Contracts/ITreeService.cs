using TransitiveClosureTable.Application.Dto;

namespace TransitiveClosureTable.Application.Services.Contracts;

/// <summary>
/// Service interface for managing Tree entities and their node hierarchies.
/// </summary>
public interface ITreeService
{
    /// <summary>
    /// Gets an existing tree by name, or creates a new one if it does not exist.
    /// Ensures a root node and transitive closure entries are created as needed.
    /// </summary>
    /// <param name="name">The name of the tree.</param>
    /// <returns>A <see cref="TreeDto"/> representing the tree and its nested nodes.</returns>
    Task<TreeDto> GetOrCreateAsync(string name);
}