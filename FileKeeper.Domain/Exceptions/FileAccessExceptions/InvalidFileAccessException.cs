namespace FileKeeper.Domain.Exceptions.FileAccessExceptions;

public class InvalidFileAccessException: DomainException
{
    public InvalidFileAccessException(string message) : base(message) { }
}