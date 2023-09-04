using Bogus;
using PlatesOrganiser.Domain.Entities;

namespace PlatesOrganiser.Fakes;

public static partial class Fake
{
    private static Faker<PlateUser> _plateUser =
            new Faker<PlateUser>()
                .RuleFor(x => x.UserName, f => f.Person.UserName);

    public static PlateUser PlateUser() => _plateUser.Generate();
}
