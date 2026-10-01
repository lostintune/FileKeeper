namespace FileKeeper.Application.Files.Common;

public record FileDownloadDto
{
    public Stream Content { get; init; } = Stream.Null;
    public string FileName { get; init; } = string.Empty;
}