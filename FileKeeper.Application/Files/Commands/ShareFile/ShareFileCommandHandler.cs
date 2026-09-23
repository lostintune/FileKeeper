using FileKeeper.Application.Common.Exceptions.Files;
using FileKeeper.Application.Common.Interfaces.Files;
using MediatR;

namespace FileKeeper.Application.Files.Commands.ShareFile;

public class ShareFileCommandHandler: IRequestHandler<ShareFileCommand>
{
    private readonly IFileRepository _fileRepository;
    private readonly IFileAccessRepository _fileAccessRepository;
    
    public ShareFileCommandHandler(IFileRepository fileRepository, IFileAccessRepository fileAccessRepository)
    {
        _fileRepository = fileRepository;
        _fileAccessRepository = fileAccessRepository;
    }

    public async Task Handle(ShareFileCommand request, CancellationToken cancellationToken)
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
        
        if (await _fileAccessRepository.HasAccessAsync(request.TargetUserId, request.FileId))
        {
            throw new FileAlreadySharedException(request.FileId, request.TargetUserId);
        }

        var shareAccess = file.ShareAccess(request.TargetUserId);
        await _fileAccessRepository.ShareAccessAsync(shareAccess);
    }
}