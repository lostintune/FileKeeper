namespace FileKeeper.Domain.Exceptions.FileExceptions;

public class FileAlreadyDeletedException : DomainException
{
    public FileAlreadyDeletedException() : base("File is already deleted")
    {
    }
}