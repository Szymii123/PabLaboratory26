using AppCore.Interfaces;
using AppCore.Models;
using AppCore.ValueObjects;

namespace Infrastructure.Memory;

public class MemoryPersonRepository : MemoryGenericRepository<Person>, IPersonRepository
{
    public MemoryPersonRepository()
    {
        var adamId = Guid.NewGuid();
        var ewaId = Guid.NewGuid();

        _data[adamId] = new Person
        {
            Id = adamId,
            FirstName = "Adam",
            LastName = "Nowak",
            Gender = Gender.Male,
            BirthDate = new DateTime(1970, 1, 1),
            Email = "adam.nowak@mail.pl",
            Phone = "+48 123456978",
            Status = ContactStatus.Active,
        };
        
        _data[ewaId] = new Person
        {
            Id = ewaId,
            FirstName = "Ewa",
            LastName = "Kowalska",
            Gender = Gender.Male,
            BirthDate = new DateTime(1970, 1, 1),
            Email = "ewa.kowalska@mail.pl",
            Phone = "+48 978456123",
            Status = ContactStatus.Active,
        };
    }

    public Task<IEnumerable<Person>> FindByEmployerAsync(Guid employerId)
    {
        return Task.FromResult(_data.Values.Where(p => p.EmployerId == employerId));
    }

    public Task<IEnumerable<Person>> FindByOrganizationAsync(Guid organizationId)
    {
        return Task.FromResult(_data.Values.Where(p => p.OrganizationId == organizationId));
    }

    public Task<IEnumerable<Person>> SearchAsync(string query)
    {
        query = query.Trim();
        if (string.IsNullOrWhiteSpace(query))
            return Task.FromResult(_data.Values.AsEnumerable());

        var filtered = _data.Values.Where(p => 
            p.FirstName.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            p.LastName.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            p.Email.Contains(query, StringComparison.OrdinalIgnoreCase)
        );

        return Task.FromResult(filtered);
    }
}