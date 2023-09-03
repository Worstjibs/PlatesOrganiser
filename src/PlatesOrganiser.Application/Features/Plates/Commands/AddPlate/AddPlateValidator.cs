using FluentValidation;

namespace PlatesOrganiser.Application.Features.Plates.Commands.AddPlate;

internal class AddPlateValidator : AbstractValidator<AddPlateCommand>
{
    public AddPlateValidator()
    {
        RuleFor(x => x.MasterReleaseId)
            .GreaterThan(0)
            .WithMessage("A valid Master Release Id must be provided");
    }
}
