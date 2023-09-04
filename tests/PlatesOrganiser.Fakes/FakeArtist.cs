using Bogus;
using ParkSquare.Discogs.Dto;

namespace PlatesOrganiser.Fakes;

public static partial class Fake
{
    private static Faker<Artist> _artistFake =
            new Faker<Artist>()
                .RuleFor(x => x.Name, f => f.Person.FullName);

    public static IEnumerable<Artist> Artists(int count)
    {
        return _artistFake.Generate(count);
    }
}
