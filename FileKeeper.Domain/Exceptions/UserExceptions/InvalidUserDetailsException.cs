namespace FileKeeper.Domain.Exceptions.UserExceptions;

public class InvalidUserDetailsException : DomainException
{
    public InvalidUserDetailsException(string message) : base(message) { }
}