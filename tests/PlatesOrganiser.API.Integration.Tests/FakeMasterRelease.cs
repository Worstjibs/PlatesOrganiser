using Bogus;
using ParkSquare.Discogs.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatesOrganiser.API.Integration.Tests;

public partial class Fake
{
    private static Faker<MasterRelease> _masterReleaseFake =
            new Faker<MasterRelease>()
                .RuleFor(x => x.Title, f => f.Commerce.Product());

    public static MasterRelease MasterRelease(int masterReleaseId)
    {
        _masterReleaseFake.RuleFor(x => x.MainReleaseId, masterReleaseId);

        return _masterReleaseFake.Generate();
    }
}
