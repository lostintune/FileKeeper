using FileKeeper.Application.Users.Common;
using MediatR;

namespace FileKeeper.Application.Users.Commands.UpdateProfile;

public class UpdateProfileCommand: IRequest<UserDto>
{
    public Guid Id { get; set; } 
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}