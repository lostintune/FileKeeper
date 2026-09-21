namespace FileKeeper.Domain.Exceptions.FileExceptions;

public class InvalidFileDetailsException : Exception
{
    public InvalidFileDetailsException(string message) : base(message)
    {
    }
}