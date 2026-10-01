using FileKeeper.API.Services;
using FileKeeper.Application.Files.Commands.CreateFile;
using FileKeeper.Application.Files.Commands.DeleteFile;
using FileKeeper.Application.Files.Commands.ShareFile;
using FileKeeper.Application.Files.Commands.UnshareFile;
using FileKeeper.Application.Files.Commands.UpdateFile;
using FileKeeper.Application.Files.Queries.DownloadFile;
using FileKeeper.Application.Files.Queries.GetFileById;
using FileKeeper.Application.Files.Queries.GetMyFiles;
using FileKeeper.Application.Files.Queries.GetSharedFiles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FileKeeper.API.Controllers;

[Authorize]
[ApiController]
[Route("api/files")]
public class FilesController: ControllerBase
{
    private readonly IMediator _mediator;
    private readonly CurrentUserService _currentUser;

    public FilesController(IMediator mediator, CurrentUserService currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateFileCommand command)
    {
        command.CreatorId = _currentUser.UserId;
        var fileId = await _mediator.Send(command);
        return Ok(fileId);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetFileByIdQuery { FileId = id, UserId = _currentUser.UserId };
        var file = await _mediator.Send(query);
        
        return Ok(file);
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyFiles()
    {
        var query = new GetMyFilesQuery{ UserId = _currentUser.UserId };
        var files = await _mediator.Send(query);
        
        return Ok(files);
    }

    [HttpGet("shared")]
    public async Task<IActionResult> GetSharedFiles()
    {
        var query = new GetSharedFilesQuery { UserId = _currentUser.UserId };
        var files = await _mediator.Send(query);

        return Ok(files);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateFileCommand command)
    {
        command.Id = id;
        command.UserId = _currentUser.UserId;
        var file = await _mediator.Send(command);
        
        return Ok(file);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteFileCommand { Id = id, UserId = _currentUser.UserId };
        await _mediator.Send(command);

        return NoContent();
    }

    [HttpPost("{id:guid}/share")]
    public async Task<IActionResult> Share(Guid id, [FromBody] ShareFileCommand command)
    {
        command.FileId = id;
        command.CurrentUserId = _currentUser.UserId;
        await _mediator.Send(command);
        
        return NoContent();
    }

    [HttpDelete("{id:guid}/share/{targetUserId:guid}")]
    public async Task<IActionResult> Unshare(Guid id, Guid targetUserId)
    {
        var command = new UnshareFileCommand { CurrentUserId = _currentUser.UserId, FileId = id, TargetUserId = targetUserId };
        await _mediator.Send(command);
        
        return NoContent();
    }
    
    [HttpGet("{id:guid}/download")]
    public async Task<IActionResult> Download(Guid id)
    {
        var query = new DownloadFileQuery { FileId = id, UserId = _currentUser.UserId };
        var result = await _mediator.Send(query);

        return File(result.Content, "application/octet-stream", result.FileName);
    }
}