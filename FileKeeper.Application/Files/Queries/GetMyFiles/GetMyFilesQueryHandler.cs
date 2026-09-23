using FileKeeper.Application.Common.Interfaces.Files;
using FileKeeper.Application.Files.Common;
using MediatR;

namespace FileKeeper.Application.Files.Queries.GetMyFiles;

public class GetMyFilesQueryHandler: IRequestHandler<GetMyFilesQuery, List<FileDto>>
{
    private readonly IFileRepository _fileRepository;
    
    public GetMyFilesQueryHandler(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }
    
    public async Task<List<FileDto>> Handle(GetMyFilesQuery request, CancellationToken cancellationToken)
    {
        var files = await _fileRepository.GetFilesByCreatorIdAsync(request.UserId);
        
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