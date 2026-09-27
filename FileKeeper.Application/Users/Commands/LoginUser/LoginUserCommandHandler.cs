using FileKeeper.Application.Common.Exceptions;
using FileKeeper.Application.Common.Interfaces;
using FileKeeper.Application.Common.Interfaces.Users;
using FileKeeper.Application.Users.Common;
using MediatR;

namespace FileKeeper.Application.Users.Commands.LoginUser;

public class LoginUserCommandHandler: IRequestHandler<LoginUserCommand, LoginResponseDto>
{
    private readonly IIdentityService _identityService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginUserCommandHandler(IIdentityService identityService, IJwtTokenGenerator jwtTokenGenerator)
    {
        _identityService = identityService;
        _jwtTokenGenerator = jwtTokenGenerator;
    }
    
    public async Task<LoginResponseDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _identityService.ValidateCredentialsAsync(request.Email, request.Password);

        if (user == null)
        {
            throw new InvalidCredentialsException();
        }
        
        var token = _jwtTokenGenerator.GenerateToken(user);

        return new LoginResponseDto
        {
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        };
    }
}