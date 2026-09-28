namespace FileKeeper.Application.Common.Exceptions;

public class EmailAlreadyExistsException: ConflictException
{
    public EmailAlreadyExistsException(string email) : base($"Email '{email}' already exists.") { }
}