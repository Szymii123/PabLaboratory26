namespace AppCore.Interfaces.Exceptions;

public class ContactNotFoundException : Exception
{
    public ContactNotFoundException(string msg) : base(msg)
    {
        
    }
}