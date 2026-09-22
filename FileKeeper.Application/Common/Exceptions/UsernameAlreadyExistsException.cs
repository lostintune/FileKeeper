namespace FileKeeper.Application.Common.Exceptions;

public class UsernameAlreadyExistsException: Exception
{
    public UsernameAlreadyExistsException(string username) : base($"Username '{username}' already exists.") { }
}
