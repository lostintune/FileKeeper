namespace FileKeeper.Application.Common.Exceptions.Files;

public class FileAlreadySharedException: Exception
{
    public FileAlreadySharedException(Guid fileId, Guid targetUserId)
        : base($"File with ID '{fileId}' is already shared with user '{targetUserId}'.")
    {
    }
}