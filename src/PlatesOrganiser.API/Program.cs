using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlatesOrganiser.API.Seed;
using PlatesOrganiser.API.Services.CurrentUser;
using PlatesOrganiser.Application;
using PlatesOrganiser.Application.Services.CurrentUser;
using PlatesOrganiser.Infrastructure;
using PlatesOrganiser.Infrastructure.Context;
using System.IdentityModel.Tokens.Jwt;
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
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services
            .AddApplicationServices()
            .AddInfrastructureServices(builder.Configuration);

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = "http://platesorganiser.auth";
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = false,
                    ValidateAudience = false,
                    ValidateActor = false,
                    ValidateLifetime = false,
                    ValidateTokenReplay = false,

                    ValidIssuer = "https://localhost:5101",

                    NameClaimType = JwtClaimTypes.GivenName,
                    RoleClaimType = JwtClaimTypes.Role,
                };
            });

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
