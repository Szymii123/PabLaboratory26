using AppCore.Dto;
using AppCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi;

[ApiController]
[Route("/api/contacts")]

public class ContactsController(IPersonService service): ControllerBase
{

    public async Task<IActionResult> GetAllPersons(int page=1, int size=10)
    {
        return Ok(await service.FindAllPeoplePaged(page, size));
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetPerson(Guid id)
    {
        var person = await service.FindByIdAsync(id);
        return person is null ? NotFound() : Ok(person);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreatePerson(CreatePersonDto dto)
    {
        var created = await service.AddPerson(dto);
        return CreatedAtAction(nameof(GetPerson), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdatePerson(Guid id, UpdatePersonDto dto)
    {
        var person = await service.FindByIdAsync(id);
        if (person is null) return NotFound();
        var updated = await service.UpdatePerson(id, dto);
        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeletePerson(Guid id)
    {
        await service.DeleteAsync(id);
        return NoContent();
    }
    
    [HttpPost("{contactId:guid}/notes")]
    [ProducesResponseType(typeof(NoteDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddNote(
        [FromRoute] Guid contactId,
        [FromBody] CreateNoteDto dto)
    {
        var note = await service.AddNoteToPerson(contactId, dto);
        return CreatedAtAction(
            nameof(GetNotes),
            new { contactId },
            note);
    }

    [HttpGet("{contactId:guid}/notes")]
    [ProducesResponseType(typeof(IEnumerable<NoteDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetNotes([FromRoute] Guid contactId)
    {
        var person = await service.FindByIdAsync(contactId);
        return Ok(person.Notes);
    }
    
    [HttpDelete("{contactId:guid}/notes/{noteId:guid}")]
    public async Task<IActionResult> GetNotes([FromRoute] Guid contactId, [FromRoute] Guid noteId)
    {
        await service.DeleteNoteFromPerson(contactId, noteId);
        return NoContent();
    }
}