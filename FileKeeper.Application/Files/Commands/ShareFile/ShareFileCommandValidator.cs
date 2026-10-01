using FluentValidation;

namespace FileKeeper.Application.Files.Commands.ShareFile;

public class ShareFileCommandValidator : AbstractValidator<ShareFileCommand>
{
    public ShareFileCommandValidator()
    {
        RuleFor(x => x.TargetUserId)
            .NotEmpty().WithMessage("User ID is required.");
    }
}