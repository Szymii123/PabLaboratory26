using System.Security.AccessControl;

namespace AppCore.Models;

public class Company : Contact
{
    public string Name { get; set; } = string.Empty;
    public string? Nip { get; set; }
    public string? Industry { get; set; }
    public string? Phone { get; set; }
    public override string GetDisplayName()
    {
        return Name;
    }

    public string? Email { get; set; }
    public string? Website { get; set; }
    public ICollection<Person> Employees { get; set; } = new List<Person>();
}