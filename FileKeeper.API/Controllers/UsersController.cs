using FileKeeper.Application.Users.Commands.DeleteUser;
using FileKeeper.Application.Users.Commands.LoginUser;
using FileKeeper.Application.Users.Commands.RegisterUser;
using FileKeeper.Application.Users.Commands.UpdateProfile;
using FileKeeper.Application.Users.Queries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace FileKeeper.API.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController: ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        var userId = await _mediator.Send(command);
        
        return Ok(userId);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
    {
        var user = await _mediator.Send(command);
        
        return Ok(user);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetUserByIdQuery { Id = id };
        var user = await _mediator.Send(query);
    
        return Ok(user);
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateProfile(Guid id, [FromBody] UpdateProfileCommand command)
    {
        command.Id = id;
        var user = await _mediator.Send(command);
    
        return Ok(user);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteUserCommand { Id = id };
        await _mediator.Send(command);
    
        return NoContent();
    }
}