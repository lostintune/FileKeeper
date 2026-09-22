using FileKeeper.Application.Common.Exceptions;
using FileKeeper.Application.Common.Interfaces;
using FileKeeper.Domain.Entities;
using MediatR;

namespace FileKeeper.Application.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler: IRequestHandler<RegisterUserCommand, Guid>
{
    private readonly IIdentityService _identityService;
    
    public RegisterUserCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    
    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if(await _identityService.EmailExistsAsync(request.Email))
        {
            throw new EmailAlreadyExistsException(request.Email);
        }
        if(await _identityService.UsernameExistsAsync(request.Username)) 
        {
            throw new UsernameAlreadyExistsException(request.Username);
        }
        
        var user = UserEntity.Create(request.FirstName, request.LastName, request.Username, request.Email, request.PhoneNumber);
        
        return await _identityService.CreateUserAsync(user, request.Password);
    }
}