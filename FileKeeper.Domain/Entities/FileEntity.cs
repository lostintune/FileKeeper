using FileKeeper.Domain.Exceptions.FileAccessExceptions;
using FileKeeper.Domain.Exceptions.FileExceptions;

namespace FileKeeper.Domain.Entities;

public class FileEntity
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Path { get; private set; } = string.Empty;
    public Guid CreatorId { get; private set; } 
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public bool IsDeleted { get; private set; }
    
    private FileEntity() { }
    
    public static FileEntity Create(string name, Guid creatorId, string path)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidFileDetailsException("File name is invalid");
        }

        if (creatorId == Guid.Empty)
        {
            throw new InvalidFileDetailsException("Creator ID is invalid");
        }

        if (string.IsNullOrWhiteSpace(path))
        {
            throw new InvalidFileDetailsException("File path is invalid");
        }
        return new FileEntity
        {
            Id = Guid.NewGuid(),
            Name = name,
            Path = path,
            CreatorId = creatorId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsDeleted = false
        };
    }
    
    public void UpdateDetails(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidFileDetailsException("File name is invalid");
        }

        Name = name;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void SoftDelete()
    {
        if (IsDeleted)
        {
            throw new FileAlreadyDeletedException();
        }
        IsDeleted = true;
    }

    public FileAccess ShareAccess(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new InvalidFileAccessException("User ID is invalid");
        }
        if (userId == CreatorId)
        {
            throw new InvalidFileAccessException("Creator cannot share access to themselves");
        }

        if (IsDeleted)
        {
            throw new FileAlreadyDeletedException();
        }
        
        return FileAccess.Create(userId, Id);
    }
    
}