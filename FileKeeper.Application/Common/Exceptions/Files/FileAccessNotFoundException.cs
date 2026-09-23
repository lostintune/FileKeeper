namespace FileKeeper.Application.Common.Exceptions.Files;

public class FileAccessNotFoundException: Exception
{
    public FileAccessNotFoundException(Guid fileId, Guid targetUserId)
        : base($"File with ID '{fileId}' is already unshared with user '{targetUserId}'.")
    {
    }
}