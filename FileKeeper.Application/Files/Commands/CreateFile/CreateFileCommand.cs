using MediatR;
using Microsoft.AspNetCore.Http;

namespace FileKeeper.Application.Files.Commands.CreateFile;

public class CreateFileCommand: IRequest<Guid>
{
    public string Name { get; set; } = string.Empty;
    public Guid CreatorId { get; set; }
    public IFormFile File { get; set; } = null!;
}