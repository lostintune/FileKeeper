using FluentValidation;

namespace FileKeeper.Application.Files.Commands.CreateFile;

public class CreateFileCommandValidator : AbstractValidator<CreateFileCommand>
{
    public CreateFileCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("File name is required.")
            .MaximumLength(255).WithMessage("File name must not exceed 255 characters.");

        RuleFor(x => x.File)
            .NotEmpty().WithMessage("File content is required.")
            .Must(file => file.Length <= 10 * 1024 * 1024)
            .WithMessage("File size must not exceed 10 MB.");
    }
}