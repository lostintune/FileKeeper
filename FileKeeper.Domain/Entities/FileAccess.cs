using FileKeeper.Domain.Exceptions.FileAccessExceptions;

namespace FileKeeper.Domain.Entities;

public class FileAccess
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid FileId { get; private set; }
    
    private FileAccess() { }
    
    public static FileAccess Create(Guid userId, Guid fileId)
    {
        if (userId == Guid.Empty)
        {
            throw new InvalidFileAccessException("User ID is invalid");
        }

        if (fileId == Guid.Empty)
        {
            throw new InvalidFileAccessException("File ID is invalid");
        }

        return new FileAccess
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            FileId = fileId
        };
    }
    
}