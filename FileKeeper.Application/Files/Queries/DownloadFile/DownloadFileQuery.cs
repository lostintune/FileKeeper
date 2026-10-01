using FileKeeper.Application.Files.Common;
using MediatR;

namespace FileKeeper.Application.Files.Queries.DownloadFile;

public class DownloadFileQuery: IRequest<FileDownloadDto>
{
    public Guid FileId { get; set; }
    public Guid UserId { get; set; }
}