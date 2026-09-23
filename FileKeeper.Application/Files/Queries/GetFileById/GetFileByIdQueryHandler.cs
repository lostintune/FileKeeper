using FileKeeper.Application.Common.Exceptions.Files;
using FileKeeper.Application.Common.Interfaces.Files;
using FileKeeper.Application.Files.Common;
using MediatR;

namespace FileKeeper.Application.Files.Queries.GetFileById;

public class GetFileByIdQueryHandler: IRequestHandler<GetFileByIdQuery, FileDto>
{
    private readonly IFileRepository _fileRepository;
    private readonly IFileAccessRepository _fileAccessRepository;

    public GetFileByIdQueryHandler(IFileRepository fileRepository, IFileAccessRepository fileAccessRepository)
    {
        _fileRepository = fileRepository;
        _fileAccessRepository = fileAccessRepository;
    }

    public async Task<FileDto> Handle(GetFileByIdQuery request, CancellationToken cancellationToken)
    {
        var file = await _fileRepository.GetFileAsync(request.FileId);
        
        if (file == null)
        {
            throw new FileEntityNotFoundException(request.FileId);
        }
        
        if(file.CreatorId != request.UserId && !await _fileAccessRepository.HasAccessAsync(request.UserId, request.FileId))
        {
            throw new ForbiddenAccessException(request.UserId, request.FileId);
        }

        return new FileDto
        {
            Id = file.Id,
            Name = file.Name,
            CreatorId = file.CreatorId,
            CreatedAt = file.CreatedAt,
            UpdatedAt = file.UpdatedAt
        };
    }
}