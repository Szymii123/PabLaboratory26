using AppCore.Dto;
using AppCore.Interfaces;
using AppCore.Models;

namespace Infrastructure.Memory;

public class MemoryCompanyRepository(IPersonRepository personRepository) : MemoryGenericRepository<Company>, ICompanyRepository
{

    public Task<IEnumerable<Company>> FindByNameAsync(string nameOrPart)
    {
        nameOrPart = nameOrPart.Trim();
        if (string.IsNullOrWhiteSpace(nameOrPart))
            return Task.FromResult(_data.Values.AsEnumerable());

        var filtered = _data.Values.Where(c => 
            c.Name.Contains(nameOrPart, StringComparison.OrdinalIgnoreCase)
        );

        return Task.FromResult(filtered);
    }

    public Task<Company?> FindByNipAsync(string nip)
    {
        return Task.FromResult(
            _data.Values.FirstOrDefault(c => c.Nip == nip)
        );
    }

    public async Task<IEnumerable<Person>> GetEmployeesAsync(Guid companyId)
    {

        return await personRepository.FindByEmployerAsync(companyId);
    }
}