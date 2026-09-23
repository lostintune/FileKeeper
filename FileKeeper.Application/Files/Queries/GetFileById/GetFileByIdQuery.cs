using FileKeeper.Application.Files.Common;
using MediatR;

namespace FileKeeper.Application.Files.Queries.GetFileById;

public class GetFileByIdQuery: IRequest<FileDto>
{
    public Guid FileId { get; set; }
    public Guid UserId { get; set; }
}