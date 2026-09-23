using MediatR;

namespace FileKeeper.Application.Files.Commands.UnshareFile;

public class UnshareFileCommand: IRequest
{
    public Guid FileId { get; set; }
    public Guid TargetUserId { get; set; }
    public Guid CurrentUserId { get; set; }
}