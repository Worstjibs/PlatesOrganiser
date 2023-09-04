using Bogus;
using Discogs = ParkSquare.Discogs.Dto;

namespace PlatesOrganiser.Fakes;

public static partial class Fake
{
    private static Faker<Discogs.Version> _versionFake =
            new Faker<Discogs.Version>();

    public static IEnumerable<Discogs.Version> Versions(int count)
    {
        _versionFake.RuleFor(x => x.Label, f => f.Company.CompanyName());

        return _versionFake.Generate(count);
    }
}
