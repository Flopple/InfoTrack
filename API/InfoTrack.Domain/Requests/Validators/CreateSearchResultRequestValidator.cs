using FluentValidation;

namespace InfoTrack.Domain.Requests.Validators;

public class CreateSearchResultRequestValidator : AbstractValidator<CreateSearchResultRequest>
{
    public CreateSearchResultRequestValidator()
    {
        RuleFor(x => x.Keywords)
            .NotEmpty()
            .WithMessage("Keywords must not be empty")
            .MaximumLength(250)
            .WithMessage("Keywords must not exceed 250 characters");

        RuleFor(x => x.SearchTarget)
            .NotEmpty()
            .WithMessage("Search target must not be empty")
            .MaximumLength(250)
            .WithMessage("Search target must not exceed 250 characters");

        RuleFor(x => x.SearchEngine)
            .IsInEnum()
            .WithMessage("Search engine must be a valid value");

    }
}