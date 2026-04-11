using AppCore.ValueObjects;

namespace AppCore.Models;

public class Organization : Contact
{
    public string Name { get; set; } = string.Empty;
    public OrganizationType Type { get; set; }
    public ICollection<Person> Members { get; set; } = new List<Person>();
    public override string GetDisplayName()
    {
        return Name;
    }
}