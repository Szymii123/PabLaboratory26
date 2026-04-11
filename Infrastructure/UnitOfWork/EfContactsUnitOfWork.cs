using AppCore.Interfaces;
using Infrastructure.Context;

namespace Infrastructure.UnitOfWork;

public class EfContactsUnitOfWork(
    IPersonRepository personRepository,
    IOrganizationRepository organizationRepository,
    ICompanyRepository companyRepository,
    ContactsDbContext context
): IContactUnitOfWork
{
    public ValueTask DisposeAsync()
    {
        return context.DisposeAsync();
    }
    public IPersonRepository Persons => personRepository;
    public  ICompanyRepository Companies => companyRepository;
    public IOrganizationRepository Organizations => organizationRepository;
    public Task<int> SaveChangesAsync()
    {
        return context.SaveChangesAsync();
    }

    public Task BeginTransactionAsync()
    {
        return context.Database.BeginTransactionAsync();
    }

    public Task CommitTransactionAsync()
    {
        return context.Database.CommitTransactionAsync();
    }

    public Task RollbackTransactionAsync()
    {
        return context.Database.RollbackTransactionAsync();
    }
}
