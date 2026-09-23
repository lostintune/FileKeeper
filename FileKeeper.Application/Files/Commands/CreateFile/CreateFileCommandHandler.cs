using FileKeeper.Application.Common.Interfaces.Files;
using FileKeeper.Domain.Entities;
using MediatR;

namespace FileKeeper.Application.Files.Commands.CreateFile;

public class CreateFileCommandHandler: IRequestHandler<CreateFileCommand, Guid>
{
    private readonly IFileRepository _fileRepository;
    private readonly IFileStorage _fileStorageService;
    
    public CreateFileCommandHandler(IFileRepository fileRepository, IFileStorage fileStorageService)
    {
        _fileRepository = fileRepository;
        _fileStorageService = fileStorageService;
    }
    
    public async Task<Guid> Handle(CreateFileCommand request, CancellationToken cancellationToken)
    {
        var fileExtension = Path.GetExtension(request.File.FileName); // get extension of the file
        var storageFileName = $"{Guid.NewGuid()}{fileExtension}"; // generate unique name for the file in storage
        
        var filePath = await _fileStorageService.SaveFileAsync(storageFileName, request.File.OpenReadStream());
        
        var file = FileEntity.Create(request.Name, request.CreatorId, filePath);
        
        await _fileRepository.CreateFileAsync(file);
        
        return file.Id;
    }
}