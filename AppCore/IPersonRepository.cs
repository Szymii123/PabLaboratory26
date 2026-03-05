using AppCore.Models;

namespace AppCore.Interfaces;

public interface IPersonRepository : IGenericRepositoryAsync<Person>
{
    Task<IEnumerable<Person>> FindByEmployerAsync(Guid companyId);
    Task<IEnumerable<Person>> FindByOrganizationAsync(Guid organizationId);
}