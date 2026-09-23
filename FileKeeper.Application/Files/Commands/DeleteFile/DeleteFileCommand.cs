using MediatR;

namespace FileKeeper.Application.Files.Commands.DeleteFile;

public class DeleteFileCommand: IRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
}