using FluentValidation;

namespace PlatesOrganiser.Application.Features.Collections.Commands.AddCollection;

internal class AddCollectionValidator : AbstractValidator<AddCollectionCommand>
{
    public AddCollectionValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Collection name cannot be null or empty");
    }
}
