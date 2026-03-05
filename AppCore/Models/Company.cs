namespace AppCore.Models;

public class Company : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string? Nip { get; set; }
}