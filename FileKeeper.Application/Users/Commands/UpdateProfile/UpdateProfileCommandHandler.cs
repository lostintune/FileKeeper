using FileKeeper.Application.Common.Exceptions;
using FileKeeper.Application.Common.Interfaces;
using FileKeeper.Application.Users.Common;
using MediatR;

namespace FileKeeper.Application.Users.Commands.UpdateProfile;

public class UpdateProfileCommandHandler: IRequestHandler<UpdateProfileCommand, UserDto>
{
    private readonly IIdentityService _identityService;
    
    public UpdateProfileCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    
    public async Task<UserDto> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _identityService.GetByIdAsync(request.Id);
        
        if (user == null)
        {
            throw new UserNotFoundException(request.Id);
        }
        if (await _identityService.UsernameExistsAsync(request.Username) && user.Username != request.Username)
        {
            throw new UsernameAlreadyExistsException(request.Username);
        }
        
        user.UpdateProfile(request.FirstName, request.LastName, request.Username, request.PhoneNumber);
        
        await _identityService.UpdateProfileAsync(user);
        
        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Username = user.Username,
            PhoneNumber = user.PhoneNumber,
            Email = user.Email
        };
    }
}