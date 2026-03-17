using AppCore.Dto;
using AppCore.Interfaces;
using AppCore.Models;
using AppCore.ValueObjects;

namespace Infrastructure.Memory;

public class MemoryOrganizationRepository(IPersonRepository personRepository) : MemoryGenericRepository<Organization>, IOrganizationRepository
{

    public Task<IEnumerable<Organization>> FindByTypeAsync(OrganizationType type)
    {
        return Task.FromResult(_data.Values.Where(o => o.Type == type));
    }

    public async Task<IEnumerable<Person>> GetPeopleInOrganizationAsync(Guid organizationId)
    {
        return await personRepository.FindByOrganizationAsync(organizationId);
    }
}