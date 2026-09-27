using FileKeeper.Application.Users.Common;
using MediatR;

namespace FileKeeper.Application.Users.Commands.LoginUser;

public class LoginUserCommand:IRequest<LoginResponseDto>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}