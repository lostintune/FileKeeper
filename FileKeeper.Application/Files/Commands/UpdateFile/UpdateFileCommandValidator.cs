using FluentValidation;

namespace FileKeeper.Application.Files.Commands.UpdateFile;

public class UpdateFileCommandValidator : AbstractValidator<UpdateFileCommand>
{
    public UpdateFileCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("File name is required.")
            .MaximumLength(255).WithMessage("File name must not exceed 255 characters.");
    }
}