using AppCore.ValueObjects;

namespace AppCore.Models;

public class Organization : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public OrganizationType Type { get; set; }
}