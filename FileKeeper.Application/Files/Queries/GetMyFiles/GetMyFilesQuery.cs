using FileKeeper.Application.Files.Common;
using MediatR;

namespace FileKeeper.Application.Files.Queries.GetMyFiles;

public class GetMyFilesQuery: IRequest<List<FileDto>>
{
    public Guid UserId { get; set; }
}