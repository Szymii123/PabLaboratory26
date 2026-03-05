using AppCore.Models;
using AppCore.ValueObjects;

namespace AppCore.Interfaces;

public interface IOrganizationRepository : IGenericRepositoryAsync<Organization>
{
    Task<IEnumerable<Organization>> FindByTypeAsync(OrganizationType type);
    Task<IEnumerable<Person>> GetPeopleInOrganizationAsync(Guid organizationId);
}