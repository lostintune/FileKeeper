namespace FileKeeper.Application.Files.Common;

public record FileDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public Guid CreatorId { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}