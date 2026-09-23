namespace FileKeeper.Application.Common.Interfaces.Files;

public interface IFileStorage
{
    Task<string> SaveFileAsync(string fileName, Stream fileStream);
    Task DeleteFileAsync(string filePath);
    Task<Stream> GetFileAsync(string filePath);
}