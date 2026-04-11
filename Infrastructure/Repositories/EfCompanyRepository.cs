using AppCore.Interfaces;
using AppCore.Models;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EfCompanyRepository(ContactsDbContext context): 
    EfGenericRepository<Company>(context.Companies), 
    ICompanyRepository
{

    public async Task<IEnumerable<Company>> FindByNameAsync(string nameOrPart)
    {
        return await context.Companies
            .Where(x => x.Name.Contains(nameOrPart, StringComparison.CurrentCultureIgnoreCase))
            .ToListAsync();
    }

    public async Task<Company?> FindByNipAsync(string nip)
    {
        return await context.Companies.FirstOrDefaultAsync(x => x.Nip == nip);
    }

    public async Task<IEnumerable<Person>> GetEmployeesAsync(Guid companyId)
    {
        return await context.People.Where(x => x.EmployerId == companyId).ToListAsync();
    }
}
