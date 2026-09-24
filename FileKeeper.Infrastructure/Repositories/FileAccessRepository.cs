using FileKeeper.Application.Common.Interfaces.Files;
using FileKeeper.Domain.Entities;
using FileKeeper.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FileKeeper.Infrastructure.Repositories;

public class FileAccessRepository: IFileAccessRepository
{
    private readonly AppDbContext _context;
    
    public FileAccessRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task ShareAccessAsync(FileAccessEntity fileAccess)
    {
        _context.FileAccesses.Add(fileAccess);
        await _context.SaveChangesAsync();
    }

    public async Task UnshareAccessAsync(Guid userId, Guid fileId)
    {
        var fileAccess = await _context.FileAccesses
            .FirstOrDefaultAsync(fa => fa.UserId == userId && fa.FileId == fileId);
        
        if (fileAccess != null)
        {
            _context.FileAccesses.Remove(fileAccess);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<FileEntity>> GetSharedFilesAsync(Guid userId)
    {
        return await _context.FileAccesses
            .Where(fa => fa.UserId == userId)
            .Join(
                _context.Files,
                fa => fa.FileId,
                f => f.Id,
                (fa, f) => f
            )
            .ToListAsync();
    }

    public async Task<bool> HasAccessAsync(Guid userId, Guid fileId)
    {
        return await _context.FileAccesses.AnyAsync(fa => fa.UserId == userId && fa.FileId == fileId);
    }
}