using FileKeeper.Application.Common.Interfaces.Files;
using FileKeeper.Application.Files.Common;
using MediatR;

namespace FileKeeper.Application.Files.Queries.GetSharedFiles;

public class GetSharedFilesQueryHandler: IRequestHandler<GetSharedFilesQuery, List<FileDto>>
{
    private readonly IFileAccessRepository _fileAccessRepository;
    
    public GetSharedFilesQueryHandler(IFileAccessRepository fileAccessRepository)
    {
        _fileAccessRepository = fileAccessRepository;
    }
    
    public async Task<List<FileDto>> Handle(GetSharedFilesQuery request, CancellationToken cancellationToken)
    {
        var files = await _fileAccessRepository.GetSharedFilesAsync(request.UserId);
        
        return files.Select(file => new FileDto
        {
            Id = file.Id,
            Name = file.Name,
            CreatorId = file.CreatorId,
            CreatedAt = file.CreatedAt,
            UpdatedAt = file.UpdatedAt
        }).ToList();
    }
}