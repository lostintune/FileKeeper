using MediatR;

namespace FileKeeper.Application.Files.Commands.ShareFile;

public class ShareFileCommand: IRequest
{
    public Guid FileId { get; set; }
    public Guid TargetUserId { get; set; }
    public Guid CurrentUserId { get; set; }
}