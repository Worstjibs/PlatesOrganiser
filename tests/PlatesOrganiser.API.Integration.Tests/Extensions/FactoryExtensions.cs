using Docker.DotNet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlatesOrganiser.Domain.Entities;
using PlatesOrganiser.Domain.Repositories;
using PlatesOrganiser.Infrastructure.Context;

namespace PlatesOrganiser.API.Integration.Tests.Extensions;

public static class FactoryExtensions
{
    public static async Task<Plate?> GetPlateById(this WebApiFactory factory, Guid id)
    {
        var scope = factory.Services.CreateScope();

        var repository = scope.ServiceProvider.GetRequiredService<IPlateRepository>();

        return await repository.GetPlateByIdAsync(id);
    }

    public static async Task<PlateUser?> GetPlateUserById(this WebApiFactory factory, Guid id)
    {
        var scope = factory.Services.CreateScope();

        var repository = scope.ServiceProvider.GetRequiredService<IPlateUserRepository>();

        return await repository.GetUserByIdAsync(id);
    }

    public static async Task<PlateCollection?> GetCollectionById(this WebApiFactory factory, Guid id)
    {
        var scope = factory.Services.CreateScope();

        var repository = scope.ServiceProvider.GetRequiredService<IPlateCollectionRepository>();

        return await repository.GetCollectionByIdAsync(id, CancellationToken.None);
    }

    public static async Task<PlateCollection> AddCollectionAsync(this WebApiFactory factory, string collectionName, Guid userId)
    {
        var scope = factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PlatesContext>();

        var user = await context.Users.FirstAsync(x => x.Id == userId);
        var collection = new PlateCollection { Name = collectionName, User = user };
        user.Collections.Add(collection);

        await context.SaveChangesAsync();

        return collection;
    }
}