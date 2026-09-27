namespace FileKeeper.Application.Users.Common;

public record LoginResponseDto
{
    public string Token { get; init; } = string.Empty;
    public DateTime ExpiresAt { get; init; }
}