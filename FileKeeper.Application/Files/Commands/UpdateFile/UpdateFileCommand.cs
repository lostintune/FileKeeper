using FileKeeper.Application.Files.Common;
using MediatR;

namespace FileKeeper.Application.Files.Commands.UpdateFile;

public class UpdateFileCommand: IRequest<FileDto>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid UserId { get; set; }
}