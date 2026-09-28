using FileKeeper.Application.Files.Commands.CreateFile;
using FileKeeper.Application.Files.Commands.DeleteFile;
using FileKeeper.Application.Files.Commands.ShareFile;
using FileKeeper.Application.Files.Commands.UnshareFile;
using FileKeeper.Application.Files.Commands.UpdateFile;
using FileKeeper.Application.Files.Queries.GetFileById;
using FileKeeper.Application.Files.Queries.GetMyFiles;
using FileKeeper.Application.Files.Queries.GetSharedFiles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FileKeeper.API.Controllers;

[ApiController]
[Route("api/files")]
public class FilesController: ControllerBase
{
    private readonly IMediator _mediator;

    public FilesController (IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateFileCommand command)
    {
        var fileId = await _mediator.Send(command);
        return Ok(fileId);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, [FromQuery] Guid userId)
    {
        var query = new GetFileByIdQuery { FileId = id, UserId = userId };
        var file = await _mediator.Send(query);
        
        return Ok(file);
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyFiles([FromQuery] Guid userId)
    {
        var query = new GetMyFilesQuery{ UserId = userId };
        var files = await _mediator.Send(query);
        
        return Ok(files);
    }

    [HttpGet("shared")]
    public async Task<IActionResult> GetSharedFiles([FromQuery] Guid userId)
    {
        var query = new GetSharedFilesQuery { UserId = userId };
        var files = await _mediator.Send(query);

        return Ok(files);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromQuery] Guid userId, [FromBody] UpdateFileCommand command)
    {
        command.Id = id;
        command.UserId = userId;
        var file = await _mediator.Send(command);
        
        return Ok(file);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, [FromQuery] Guid userId)
    {
        var command = new DeleteFileCommand { Id = id, UserId = userId };
        await _mediator.Send(command);

        return NoContent();
    }

    [HttpPost("{id:guid}/share")]
    public async Task<IActionResult> Share(Guid id, [FromQuery] Guid userId, [FromBody] ShareFileCommand command)
    {
        command.FileId = id;
        command.CurrentUserId = userId;
        await _mediator.Send(command);
        
        return NoContent();
    }

    [HttpDelete("{id:guid}/share/{targetUserId:guid}")]
    public async Task<IActionResult> Unshare(Guid id, Guid targetUserId, [FromQuery] Guid userId)
    {
        var command = new UnshareFileCommand { CurrentUserId = userId, FileId = id, TargetUserId = targetUserId };
        await _mediator.Send(command);
        
        return NoContent();
    }
}