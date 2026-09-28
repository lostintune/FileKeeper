namespace FileKeeper.Application.Common.Exceptions.Files;

public class FileEntityNotFoundException: NotFoundException
{
    public FileEntityNotFoundException(Guid fileId) : base($"File with ID '{fileId}' was not found.")
    {
    }
}