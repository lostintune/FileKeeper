namespace FileKeeper.Domain.Exceptions.UserExceptions;

public class UserAlreadyDeletedException: DomainException
{
    public UserAlreadyDeletedException() : base("User is already deleted.") { }
}