using PlatesOrganiser.Infrastructure.Context;

namespace PlatesOrganiser.API.Seed;

public static class Seeder
{
    public static Task SeedDatabaseAsync(PlatesContext context)
    {
        return Task.CompletedTask;
    }
}
