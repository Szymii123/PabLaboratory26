using AppCore.Interfaces;
using AppCore.Models;
using AppCore.ValueObjects;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EfOrganizationRepository(ContactsDbContext context) 
    :EfGenericRepository<Organization>(context.Organizations), IOrganizationRepository
{
    public async Task<IEnumerable<Organization>> FindByTypeAsync(OrganizationType type)
    {
        return await context.Organizations.Where(x => x.Type ==  type).ToListAsync();
    }

    public async Task<IEnumerable<Person>> GetPeopleInOrganizationAsync(Guid organizationId)
    {
        return await context.People.Where(x => x.OrganizationId == organizationId).ToListAsync();
    }
}