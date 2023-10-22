namespace AnonymousChatBot.Service.Errors;

public class UnknownUpdateException : Exception
{
    public UnknownUpdateException(string message)
        : base(message)
    {
    }
}