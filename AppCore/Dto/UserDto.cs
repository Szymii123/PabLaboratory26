using System.Collections.Generic;
using AppCore.Interfaces;

namespace AppCore.Dto;


public class UserDto
{
    public required string Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string Department { get; init; }
    public SystemUserStatus Status { get; init; }
    public required IEnumerable<string> Roles { get; init; }
}
