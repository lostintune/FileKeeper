using FileKeeper.API.Services;
using FileKeeper.Application.Users.Commands.DeleteUser;
using FileKeeper.Application.Users.Commands.LoginUser;
using FileKeeper.Application.Users.Commands.RegisterUser;
using FileKeeper.Application.Users.Commands.UpdateProfile;
using FileKeeper.Application.Users.Queries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FileKeeper.API.Controllers;

[Authorize]
[ApiController]
[Route("api/users")]
public class UsersController: ControllerBase
{
    private readonly IMediator _mediator;
    private readonly CurrentUserService _currentUser;

    public UsersController(IMediator mediator, CurrentUserService currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        var userId = await _mediator.Send(command);
        
        return Ok(userId);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
    {
        var user = await _mediator.Send(command);
        
        return Ok(user);
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var query = new GetUserByIdQuery { Id = _currentUser.UserId };
        var user = await _mediator.Send(query);
    
        return Ok(user);
    }
    
    [HttpPut("me")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileCommand command)
    {
        command.Id = _currentUser.UserId;
        var user = await _mediator.Send(command);
    
        return Ok(user);
    }
    
    [HttpDelete("me")]
    public async Task<IActionResult> Delete()
    {
        var command = new DeleteUserCommand { Id = _currentUser.UserId };
        await _mediator.Send(command);
    
        return NoContent();
    }
}