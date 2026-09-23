namespace FileKeeper.Application.Common.Exceptions.Files;

public class ForbiddenAccessException: Exception
{
    public ForbiddenAccessException(Guid userId, Guid fileId) : base($"User with ID '{userId}' does not have access to file with ID '{fileId}'.")
    {
    }
}