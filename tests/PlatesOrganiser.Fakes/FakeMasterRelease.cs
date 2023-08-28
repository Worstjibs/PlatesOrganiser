using Bogus;
using ParkSquare.Discogs.Dto;

namespace PlatesOrganiser.Fakes;

public partial class Fake
{
    private static Faker<MasterRelease> _masterReleaseFake =
            new Faker<MasterRelease>()
                .RuleFor(x => x.Title, f => f.Commerce.Product())
                .RuleFor(x => x.Year, f => f.Random.Int(min: 1950, max: 2023))
                .RuleFor(x => x.Artists, f => Artists(10));

    public static MasterRelease MasterRelease(int masterReleaseId)
    {
        _masterReleaseFake.RuleFor(x => x.MasterId, masterReleaseId);

        return _masterReleaseFake.Generate();
    }
}
