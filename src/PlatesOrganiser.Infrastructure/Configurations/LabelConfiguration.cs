using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlatesOrganiser.Domain.Entities;

namespace PlatesOrganiser.Infrastructure.Configurations;

internal class LabelConfiguration : IEntityTypeConfiguration<Label>
{
    public void Configure(EntityTypeBuilder<Label> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasMany(x => x.Plates)
            .WithOne(x => x.PrimaryLabel)
            .HasForeignKey(x => x.PrimaryLabelId);
    }
}
