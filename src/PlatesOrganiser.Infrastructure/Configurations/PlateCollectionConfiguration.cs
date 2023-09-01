using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlatesOrganiser.Domain.Entities;

namespace PlatesOrganiser.Infrastructure.Configurations;

internal class PlateCollectionConfiguration : IEntityTypeConfiguration<PlateCollection>
{
    public void Configure(EntityTypeBuilder<PlateCollection> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasMany(x => x.Plates)
            .WithMany();

        builder.HasOne(x => x.User)
            .WithMany(x => x.Collections)
            .HasForeignKey(x => x.UserId);
    }
}
