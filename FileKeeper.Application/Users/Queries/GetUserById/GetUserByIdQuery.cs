using FileKeeper.Application.Users.Common;
using MediatR;

namespace FileKeeper.Application.Users.Queries.GetUserById;

public class GetUserByIdQuery: IRequest<UserDto>
{
    public Guid Id { get; set; }
}