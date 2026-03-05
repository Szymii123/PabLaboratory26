using AppCore.ValueObjects;

namespace AppCore.Models;

public class Address : EntityBase
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    public AddressType Type { get; set; } = AddressType.Main;
}