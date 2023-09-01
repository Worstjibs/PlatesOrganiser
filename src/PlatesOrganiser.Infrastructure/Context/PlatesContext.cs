using Microsoft.EntityFrameworkCore;
using PlatesOrganiser.Domain.Entities;
using System.Reflection;

namespace PlatesOrganiser.Infrastructure.Context;

public class PlatesContext : DbContext
{
    public PlatesContext(DbContextOptions<PlatesContext> options) : base(options)
    {
    }

    public DbSet<PlateUser> Users { get; set; }
    public DbSet<Plate> Plates { get; set; }
    public DbSet<Label> Labels { get; set; }
    public DbSet<PlateCollection> Collections { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(PlatesContext))!);
    }
}
