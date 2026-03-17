using AppCore.Dto;
using AppCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi;

[ApiController]

[Route("/api/contacts")]

public class ContactsController(IPersonService service): ControllerBase

{

    public async Task<IActionResult> GetAllPersons(int page, int size)

    {

        return Ok(await service.FindAllPeoplePaged(page, size));

    }
    
    [HttpGet("persons/{id:guid}")]
    public async Task<IActionResult> GetPerson(Guid id)
    {
        var person = await service.FindByIdAsync(id);
        return person is null ? NotFound() : Ok(person);
    }
    
    [HttpPost("persons")]
    public async Task<IActionResult> CreatePerson(CreatePersonDto dto)
    {
        var created = await service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetPerson), new { id = created.Id }, created);
    }

    [HttpPut("persons/{id:guid}")]
    public async Task<IActionResult> UpdatePerson(Guid id, UpdatePersonDto dto)
    {
        var updated = await service.UpdateAsync(id, dto);
        return Ok(updated);
    }

    [HttpDelete("persons/{id:guid}")]
    public async Task<IActionResult> DeletePerson(Guid id)
    {
        await service.DeleteAsync(id);
        return NoContent();
    }

}