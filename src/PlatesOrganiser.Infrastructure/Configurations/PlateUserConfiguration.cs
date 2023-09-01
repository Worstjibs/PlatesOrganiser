using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlatesOrganiser.Domain.Entities;

namespace PlatesOrganiser.Infrastructure.Configurations;

internal class PlateUserConfiguration : IEntityTypeConfiguration<PlateUser>
{
    public void Configure(EntityTypeBuilder<PlateUser> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasMany(x => x.Collections)
            .WithOne(x => x.User);
    }
}
