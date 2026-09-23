using FileKeeper.Application.Files.Common;
using MediatR;

namespace FileKeeper.Application.Files.Queries.GetSharedFiles;

public class GetSharedFilesQuery: IRequest<List<FileDto>>
{
    public Guid UserId { get; set; }
}