using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlatesOrganiser.Domain.Entities;

namespace PlatesOrganiser.Infrastructure.Configurations;

internal class PlateConfiguration : IEntityTypeConfiguration<Plate>
{
    public void Configure(EntityTypeBuilder<Plate> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.PrimaryLabel)
            .WithMany(x => x.Plates)
            .HasForeignKey(x => x.PrimaryLabelId);
    }
}
