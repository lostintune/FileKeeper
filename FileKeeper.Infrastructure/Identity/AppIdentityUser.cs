using Microsoft.AspNetCore.Identity;

namespace FileKeeper.Infrastructure.Identity;

public class AppIdentityUser:IdentityUser<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
}