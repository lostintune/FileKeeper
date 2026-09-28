namespace FileKeeper.Application.Common.Exceptions.Files;

public class FileAlreadySharedException: ConflictException
{
    public FileAlreadySharedException(Guid fileId, Guid targetUserId)
        : base($"File with ID '{fileId}' is already shared with user '{targetUserId}'.")
    {
    }
}