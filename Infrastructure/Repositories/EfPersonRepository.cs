using AppCore.Interfaces;
using AppCore.Models;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EfPersonRepository(ContactsDbContext context) 
    :EfGenericRepository<Person>(context.People), IPersonRepository
{
    public async Task<IEnumerable<Person>> FindByEmployerAsync(Guid employerId)
    {
        return await context.People.Where(x => x.EmployerId == employerId).ToListAsync();
    }

    public async Task<IEnumerable<Person>> FindByOrganizationAsync(Guid organizationId)
    {
        return await context.People.Where(x => x.OrganizationId == organizationId).ToListAsync();
    }

    public async Task<IEnumerable<Person>> SearchAsync(string query)
    {
        return await context.People
            .Where(x => x.FirstName.Contains(query, StringComparison.CurrentCultureIgnoreCase))
            .ToListAsync();
    }
}