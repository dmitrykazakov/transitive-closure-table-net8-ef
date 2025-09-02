namespace TransitiveClosureTable.Infrastructure.Factories.Contracts;

public interface IUnitOfWorkFactory
{
    IUnitOfWork Create();
}