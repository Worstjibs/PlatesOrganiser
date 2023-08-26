using FluentValidation;

namespace PlatesOrganiser.Application.Features.RecordQuery;

internal class GetAllValidator : AbstractValidator<GetAllQuery>
{
    public GetAllValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
    }
}
