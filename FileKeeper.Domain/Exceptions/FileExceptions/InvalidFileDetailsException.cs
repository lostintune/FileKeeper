namespace FileKeeper.Domain.Exceptions.FileExceptions;

public class InvalidFileDetailsException : DomainException
{
    public InvalidFileDetailsException(string message) : base(message)
    {
    }
}