using FileKeeper.Application.Common.Exceptions.Files;
using FileKeeper.Application.Common.Interfaces.Files;
using MediatR;

namespace FileKeeper.Application.Files.Commands.DeleteFile;

public class DeleteFileCommandHandler: IRequestHandler<DeleteFileCommand>
{
    private readonly IFileRepository _fileRepository;
    
    public DeleteFileCommandHandler(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }
    
    public async Task Handle(DeleteFileCommand request, CancellationToken cancellationToken)
    {
        var file = await _fileRepository.GetFileAsync(request.Id);
        if (file == null)
        {
            throw new FileEntityNotFoundException(request.Id);
        }
        
        var isCreator = file.CreatorId == request.UserId;
        if (!isCreator)
        {
            throw new ForbiddenAccessException(request.UserId, request.Id);
        }

        file.SoftDelete();
        
        await _fileRepository.UpdateFileAsync(file);
    }
}