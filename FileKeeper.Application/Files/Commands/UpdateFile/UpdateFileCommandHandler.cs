using FileKeeper.Application.Common.Exceptions.Files;
using FileKeeper.Application.Common.Interfaces.Files;
using FileKeeper.Application.Files.Common;
using MediatR;

namespace FileKeeper.Application.Files.Commands.UpdateFile;

public class UpdateFileCommandHandler: IRequestHandler<UpdateFileCommand, FileDto>
{
    private readonly IFileRepository _fileRepository;
    private readonly IFileAccessRepository _fileAccessRepository;

    public UpdateFileCommandHandler(IFileRepository fileRepository, IFileAccessRepository filePathRepository)
    {
        _fileRepository = fileRepository;
        _fileAccessRepository = filePathRepository;
    }
    
    public async Task<FileDto> Handle(UpdateFileCommand request, CancellationToken cancellationToken)
    {
        var file = await _fileRepository.GetFileAsync(request.Id);
        if (file == null)
        {
            throw new FileEntityNotFoundException(request.Id);
        }
        
        var hasSharedAccess = await _fileAccessRepository.HasAccessAsync(request.UserId, request.Id);
        var isCreator = request.UserId == file.CreatorId;
        
        if (!hasSharedAccess && !isCreator)
        {
            throw new ForbiddenAccessException(request.UserId, request.Id);
        }

        file.UpdateDetails(request.Name);
        
        await _fileRepository.UpdateFileAsync(file);

        return new FileDto()
        {
            Id = file.Id,
            Name = file.Name,
            CreatorId = file.CreatorId,
            CreatedAt = file.CreatedAt,
            UpdatedAt = file.UpdatedAt
        };
    }
}