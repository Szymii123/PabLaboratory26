using AppCore.Models;

namespace AppCore.Dto;

public static class PersonMapping
{
    public static PersonDto ToDto(Person person)
    {
        return new PersonDto
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            Email = person.Email,
            Phone = person.Phone,
            BirthDate = person.BirthDate,
            Gender = person.Gender,
            EmployerId = person.EmployerId,
            Position = person.Position,
            OrganizationId = person.OrganizationId,
            Notes = person.Notes.Select(x => new NoteDto(x.Id, x.Content, x.CreatedAt,  x.CreatedBy)).ToList(),
        };
    }

    public static Person ToEntity(this CreatePersonDto dto)
    {
        return new Person
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Phone = dto.Phone,
            BirthDate = dto.BirthDate,
            Gender = dto.Gender,
            EmployerId = dto.EmployerId,
            Position = dto.Position,
            OrganizationId = dto.OrganizationId,
            Address = dto.Address is null ? null : new Address
            {
                Street = dto.Address.Street,
                City = dto.Address.City,
                PostalCode = dto.Address.PostalCode,
                Country = dto.Address.Country,
                Type = dto.Address.Type,
            }
        };
    }

    public static void ApplyUpdate(this Person person, UpdatePersonDto dto)
    {
        person.FirstName = dto.FirstName is null ? person.FirstName : dto.FirstName;
        person.LastName = dto.LastName is null ? person.LastName : dto.LastName;
        person.Email = dto.Email is null ? person.Email : dto.Email;
        person.Phone = dto.Phone is null ? person.Phone : dto.Phone;
        person.BirthDate = dto.BirthDate is null ? person.BirthDate : dto.BirthDate;
        person.Gender = dto.Gender.HasValue ? person.Gender : dto.Gender.Value;
        person.EmployerId = dto.EmployerId is null ? person.EmployerId :  dto.EmployerId;
        person.Status = dto.Status is null ? person.Status : dto.Status.Value;
        person.Position = person.Position is null ? person.Position : dto.Position;

        person.Address = dto.Address is null
            ? person.Address
            : new Address
            {
                Street = dto.Address.Street,
                City = dto.Address.City,
                PostalCode = dto.Address.PostalCode,
                Country = dto.Address.Country,
                Type = dto.Address.Type,
            };
        
        person.UpdatedAt = DateTime.Now;
    }
}