using AppCore.Dto;
using AppCore.Interfaces;

namespace Infrastructure.Memory;

public class MemoryPersonService(IContactUnitOfWork unitOfWork) : IPersonService
{
    public async Task<PagedResult<PersonDto>> FindAllPeoplePaged(int page, int size)
    {
        var people = await unitOfWork.Persons.FindPagedAsync(page, size);
        var items = people.Items.Select(PersonMapping.ToDto).ToList();
        return new PagedResult<PersonDto>(items, people.TotalCount, people.Page, people.PageSize);
    }

    public async Task<PersonDto?> FindByIdAsync(Guid id)
    {
        var person = await unitOfWork.Persons.FindByIdAsync(id);
        return person is null ? null : PersonMapping.ToDto(person);
    }

    public async Task<PersonDto> AddPerson(CreatePersonDto dto)
    {
        var entity = dto.ToEntity();
        var created = await unitOfWork.Persons.AddAsync(entity);
        await unitOfWork.SaveChangesAsync();
        return PersonMapping.ToDto(created);
    }

    public async Task<PersonDto> UpdatePerson(Guid id, UpdatePersonDto dto)
    {
        var person = await unitOfWork.Persons.FindByIdAsync(id)
                     ?? throw new KeyNotFoundException($"Person with id {id} not found.");

        person.ApplyUpdate(dto);
        var updated = await unitOfWork.Persons.UpdateAsync(person);
        await unitOfWork.SaveChangesAsync();

        return PersonMapping.ToDto(updated);
    }

    public async Task DeleteAsync(Guid id)
    {
        await unitOfWork.Persons.RemoveByIdAsync(id);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task<IAsyncEnumerable<PersonDto>> FindPeopleFromCompany(Guid companyId)
    {
        var people = await unitOfWork.Persons.FindByEmployerAsync(companyId);
        return ToAsync(people.Select(PersonMapping.ToDto));
    }

    public async Task<IAsyncEnumerable<PersonDto>> FindPeopleFromOrganization(Guid organizationId)
    {
        var people = await unitOfWork.Persons.FindByOrganizationAsync(organizationId);
        return ToAsync(people.Select(PersonMapping.ToDto));
    }

    private static async IAsyncEnumerable<PersonDto> ToAsync(IEnumerable<PersonDto> data)
    {
        foreach (var item in data)
        {
            yield return item;
            await Task.Yield();
        }
    }
}