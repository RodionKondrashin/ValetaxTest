namespace ValetaxTest.Domain.Exceptions;

public class SecureException : Exception
{
    public string Type { get; }
    
    public long? Id { get; }

    public SecureException(string type, long? id, string message) : base(message)
    {
        Type = type;
        Id = id;
    }
}