using Microsoft.Extensions.DependencyInjection;
using PlatesOrganiser.Domain.Entities;
using PlatesOrganiser.Domain.Repositories;

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
}