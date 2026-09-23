using FileKeeper.Application.Common.Exceptions.Files;
using FileKeeper.Application.Common.Interfaces.Files;
using MediatR;

namespace FileKeeper.Application.Files.Commands.UnshareFile;

public class UnshareFileCommandHandler: IRequestHandler<UnshareFileCommand>
{
    private readonly IFileRepository _fileRepository;
    private readonly IFileAccessRepository _fileAccessRepository;
    
    public UnshareFileCommandHandler(IFileRepository fileRepository, IFileAccessRepository fileAccessRepository)
    {
        _fileRepository = fileRepository;
        _fileAccessRepository = fileAccessRepository;
    }
    
    public async Task Handle(UnshareFileCommand request, CancellationToken cancellationToken)
    {
        var file = await _fileRepository.GetFileAsync(request.FileId);
        if (file == null)
        {
            throw new FileEntityNotFoundException(request.FileId);
        }

        if (file.CreatorId != request.CurrentUserId)
        {
            throw new ForbiddenAccessException(request.FileId, request.CurrentUserId);
        }

        if (!await _fileAccessRepository.HasAccessAsync(request.TargetUserId, request.FileId))
        {
            throw new FileAccessNotFoundException(request.FileId, request.TargetUserId);
        }
        
        await _fileAccessRepository.UnshareAccessAsync(request.TargetUserId, request.FileId);
    }
}