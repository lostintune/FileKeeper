using FileKeeper.Domain.Entities;

namespace FileKeeper.Application.Common.Interfaces.Files;

public interface IFileAccessRepository
{
    Task ShareAccessAsync(FileAccessEntity fileAccess);
    Task UnshareAccessAsync(Guid userId, Guid fileId);
    Task<List<FileEntity>> GetSharedFilesAsync(Guid userId);
    Task<bool> HasAccessAsync(Guid userId, Guid fileId);
}