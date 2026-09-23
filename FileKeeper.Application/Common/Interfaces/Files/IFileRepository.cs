using FileKeeper.Domain.Entities;

namespace FileKeeper.Application.Common.Interfaces.Files;

public interface IFileRepository
{
    Task CreateFileAsync(FileEntity file);
    Task<FileEntity?> GetFileAsync(Guid fileId);
    Task UpdateFileAsync(FileEntity file);
    Task<List<FileEntity>> GetFilesByCreatorIdAsync(Guid creatorId);
}