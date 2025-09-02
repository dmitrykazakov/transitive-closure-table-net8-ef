using TransitiveClosureTable.Application.Services.Contracts;
using TransitiveClosureTable.Domain.Entities;
using TransitiveClosureTable.Infrastructure.Factories.Contracts;

namespace TransitiveClosureTable.Application.Services;

public class TreeService(IUnitOfWorkFactory unitOfWorkFactory) : ITreeService
{
    public async Task<Tree> GetOrCreateAsync(string name)
    {
        using var unitOfWork = unitOfWorkFactory.Create();
        await using var transaction = await unitOfWork.BeginTransactionAsync();

        var tree = await unitOfWork.Trees.GetByNameAsync(name);

        if (tree != null)
        {
            //get full tree and return
        }
        else
        {
            tree = new Tree { Name = name };

            await unitOfWork.Trees.AddAsync(tree);

            await unitOfWork.CommitAsync();

            await unitOfWork.TransitiveClosures.AddAsync(new TransitiveClosure
            {
                AncestorId = tree.Id,
                DescendantId = tree.Id,
                Depth = 0
            });
        }

        await unitOfWork.CommitAsync();
        await transaction.CommitAsync();

        return tree;
    }
}