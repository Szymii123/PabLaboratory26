using AppCore.Interfaces;

namespace Infrastructure.Memory;

public class MemoryContactUnitOfWork (
    IPersonRepository persons,
    ICompanyRepository companies,
    IOrganizationRepository organizations
    ) : IContactUnitOfWork
{
    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }

    public IPersonRepository Persons { get; } = persons;
    public ICompanyRepository Companies { get; } = companies;
    public IOrganizationRepository Organizations { get; } = organizations;
    public Task<int> SaveChangesAsync()
    {
        return Task.FromResult(0);
    }

    public Task BeginTransactionAsync()
    {
        return Task.CompletedTask;
    }

    public Task CommitTransactionAsync()
    {
        return Task.CompletedTask;
    }

    public Task RollbackTransactionAsync()
    {
        return Task.CompletedTask;
    }
}