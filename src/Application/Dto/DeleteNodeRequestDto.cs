namespace TransitiveClosureTable.Application.Dto
{
    /// <summary>
    ///     Request DTO for deleting a node from a tree.
    /// </summary>
    /// <remarks>
    ///     The node must not be a root node and must not have any child nodes.
    ///     This DTO is used by <see cref="TransitiveClosureTable.Application.Services.Contracts.INodeService.DeleteAsync"/> method.
    /// </remarks>
    public class DeleteNodeRequestDto
    {
        /// <summary>
        ///     The unique identifier of the node to delete.
        /// </summary>
        public int Id { get; set; }
    }
}