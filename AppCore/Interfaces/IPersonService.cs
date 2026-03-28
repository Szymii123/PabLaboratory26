using AppCore.Dto;
using AppCore.Models;

namespace AppCore.Interfaces;

public interface IPersonService
{
    Task<PagedResult<PersonDto>> FindAllPeoplePaged(int page, int size);
    Task<PersonDto?> FindByIdAsync(Guid id);
    Task<PersonDto> AddPerson(CreatePersonDto dto);
    Task<PersonDto> UpdatePerson(Guid id, UpdatePersonDto dto);
    Task DeleteAsync(Guid id);
    Task<IAsyncEnumerable<PersonDto>> FindPeopleFromCompany(Guid companyId);
    Task<IAsyncEnumerable<PersonDto>> FindPeopleFromOrganization(Guid organizationId);
    Task<Note> AddNoteToPerson(Guid personId, CreateNoteDto createNoteDto);
    Task DeleteNoteFromPerson(Guid personId, Guid noteId);
}