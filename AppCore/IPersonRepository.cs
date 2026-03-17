using AppCore.Models;

namespace AppCore.Interfaces;

public interface IPersonRepository : IGenericRepositoryAsync<Person>
{
    Task<IEnumerable<Person>> FindByEmployerAsync(Guid employerId);
    Task<IEnumerable<Person>> FindByOrganizationAsync(Guid organizationId);
    Task<IEnumerable<Person>> SearchAsync(string query);
}