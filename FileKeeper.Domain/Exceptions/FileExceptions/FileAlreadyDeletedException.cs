namespace FileKeeper.Domain.Exceptions.FileExceptions;

public class FileAlreadyDeletedException : Exception
{
    public FileAlreadyDeletedException() : base("File is already deleted")
    {
    }
}