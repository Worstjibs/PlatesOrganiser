using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ParkSquare.Discogs;
using PlatesOrganiser.API.RecordQuerying.Services;
using PlatesOrganiser.Domain.Repositories;
using PlatesOrganiser.Infrastructure.Context;
using PlatesOrganiser.Infrastructure.Repositories;
using PlatesOrganiser.Infrastructure.Services;
using System.Net;

namespace PlatesOrganiser.Infrastructure;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<ClientConfig>(config.GetSection("DiscogsClient"));

        services
            .AddSingleton<IApiQueryBuilder, ApiQueryBuilder>()
            .AddSingleton<IClientConfig, ClientConfig>(s => s.GetRequiredService<IOptions<ClientConfig>>().Value);

        services
            .AddHttpClient<IDiscogsClient, DiscogsClient>()
                .ConfigurePrimaryHttpMessageHandler(_ =>
                    new HttpClientHandler
                    {
                        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                    });

        services.AddScoped<IPlateUserRepository, PlateUserRepository>();
        services.AddScoped<IPlateRepository, PlateRepository>();
        services.AddScoped<ILabelRepository, LabelRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddTransient<IRecordQueryingService, RecordQueryingService>();

        services.AddDbContext<PlatesContext>(options => options.UseNpgsql(config.GetConnectionString("DefaultConnection")));

        return services;
    }
}
