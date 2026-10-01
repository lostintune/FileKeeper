using FileKeeper.Application.Common.Exceptions.Files;
using FileKeeper.Application.Common.Interfaces.Files;
using FileKeeper.Application.Files.Common;
using MediatR;

namespace FileKeeper.Application.Files.Queries.DownloadFile;

public class DownloadFileQueryHandler: IRequestHandler<DownloadFileQuery, FileDownloadDto>
{
    private readonly IFileRepository _fileRepository;
    private readonly IFileAccessRepository _fileAccessRepository;
    private readonly IFileStorage _fileStorage;

    public DownloadFileQueryHandler(IFileRepository fileRepository, IFileAccessRepository fileAccessRepository, IFileStorage fileStorage)
    {
        _fileRepository = fileRepository;
        _fileAccessRepository = fileAccessRepository;
        _fileStorage = fileStorage;
    }
    
    
    public async Task<FileDownloadDto> Handle(DownloadFileQuery request, CancellationToken cancellationToken)
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

        var stream = await _fileStorage.GetFileAsync(file.Path);
        var extension = System.IO.Path.GetExtension(file.Path);

        var fileName = file.Name.EndsWith(extension, StringComparison.OrdinalIgnoreCase)
            ? file.Name
            : file.Name + extension;

        return new FileDownloadDto
        {
            Content = stream,
            FileName = fileName
        };
    }
}