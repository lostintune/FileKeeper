using FileKeeper.Application.Common.Interfaces.Files;

namespace FileKeeper.Infrastructure.Storage;

public class LocalFileStorage: IFileStorage
{
    private readonly string _storagePath;
    
    public LocalFileStorage()
    {
        _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        
        if (!Directory.Exists(_storagePath)){
            Directory.CreateDirectory(_storagePath);
        }
    }
    
    public async Task<string> SaveFileAsync(string fileName, Stream fileStream)
    {
        var filePath = Path.Combine(_storagePath, fileName);
        
        using var fileOnDisk = File.Create(filePath);
        await fileStream.CopyToAsync(fileOnDisk);
        return filePath;
    }

    public Task DeleteFileAsync(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("File not found", filePath);
        }
        
        File.Delete(filePath);
        return Task.CompletedTask;
    }

    public Task<Stream> GetFileAsync(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("File not found", filePath);
        }
        
        return Task.FromResult<Stream>(File.OpenRead(filePath));
    }
}