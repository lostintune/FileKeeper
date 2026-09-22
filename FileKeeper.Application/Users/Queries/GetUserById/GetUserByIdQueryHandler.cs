using FileKeeper.Application.Common.Exceptions;
using FileKeeper.Application.Common.Interfaces;
using FileKeeper.Application.Users.Common;
using MediatR;

namespace FileKeeper.Application.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler: IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly IIdentityService _identityService;
    
    public GetUserByIdQueryHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    
    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _identityService.GetByIdAsync(request.Id);
        
        if (user == null)
        {
            throw new UserNotFoundException(request.Id);
        }
        
        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Username = user.Username,
            PhoneNumber = user.PhoneNumber,
            Email = user.Email,
            CreatedAt = user.CreatedAt
        };
    }
}