using FileKeeper.Application.Common.Exceptions;
using FileKeeper.Application.Common.Interfaces;
using FileKeeper.Application.Common.Interfaces.Users;
using MediatR;

namespace FileKeeper.Application.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler: IRequestHandler<DeleteUserCommand>
{
    private readonly IIdentityService _identityService;
    
    public DeleteUserCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _identityService.GetByIdAsync(request.Id);
        if (user == null)
        {
            throw new UserNotFoundException(request.Id);
        }
        
        user.SoftDelete();

        await _identityService.DeleteUserAsync(user);
    }
}