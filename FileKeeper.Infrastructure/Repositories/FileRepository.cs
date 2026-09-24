using FileKeeper.Application.Common.Interfaces.Files;
using FileKeeper.Domain.Entities;
using FileKeeper.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FileKeeper.Infrastructure.Repositories;

public class FileRepository: IFileRepository
{
    private readonly AppDbContext _context;
    
    public FileRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task CreateFileAsync(FileEntity file)
    {
        _context.Files.Add(file);
        await _context.SaveChangesAsync();
    }

    public async Task<FileEntity?> GetFileAsync(Guid fileId)
    {
        return await _context.Files.FindAsync(fileId);
    }

    public async Task UpdateFileAsync(FileEntity file)
    {
        _context.Files.Update(file);
        await _context.SaveChangesAsync();
    }

    public async Task<List<FileEntity>> GetFilesByCreatorIdAsync(Guid creatorId)
    {
        return await _context.Files
            .Where(f => f.CreatorId == creatorId)
            .ToListAsync();
    }
}