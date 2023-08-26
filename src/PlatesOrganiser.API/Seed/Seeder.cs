using Microsoft.EntityFrameworkCore;
using PlatesOrganiser.Domain.Entities;
using PlatesOrganiser.Infrastructure.Context;

namespace PlatesOrganiser.API.Seed;

public static class Seeder
{
    public static Guid DummyUserGuid { get; private set; } = Guid.NewGuid();

    public static async Task SeedDatabaseAsync(PlatesContext context)
    {
        if (await context.Users.AnyAsync())
        {
            await context.Users.ExecuteDeleteAsync();
        }


        var dummyUser = new PlateUser
        {
            Id = Guid.NewGuid(),
            UserName = "TestUser"
        };

        DummyUserGuid = dummyUser.Id;

        context.Users.Add(dummyUser);

        await context.SaveChangesAsync();
    }
}
