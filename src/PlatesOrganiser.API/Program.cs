using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlatesOrganiser.API.Seed;
using PlatesOrganiser.API.Services.CurrentUser;
using PlatesOrganiser.Application;
using PlatesOrganiser.Application.Services.CurrentUser;
using PlatesOrganiser.Infrastructure;
using PlatesOrganiser.Infrastructure.Context;
using PlatesOrganiser.Infrastructure.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = false;
});

builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration);

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

var clientConfig = builder.Configuration.GetSection("DiscogsClient").Get<ClientConfig>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PlatesContext>();
    await context.Database.MigrateAsync();

    await Seeder.SeedDatabaseAsync(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
