namespace AppCore.Interfaces.Exceptions;

public class ContactNoteNotFoundException : Exception
{
    public ContactNoteNotFoundException(string msg) : base(msg)
    {
        
    }
}