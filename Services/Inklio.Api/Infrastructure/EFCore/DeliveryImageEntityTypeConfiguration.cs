using Inklio.Api.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inklio.Api.Infrastructure.EFCore;

/// <summary>
/// Defines the EFCore enity configuration for an <see cref="DeliveryImage"/>.
/// </summary>
class DeliveryImageEntityTypeConfiguration : IEntityTypeConfiguration<DeliveryImage>
{
    public void Configure(EntityTypeBuilder<DeliveryImage> builder)
    {
        builder.ToTable("image", InklioContext.DbSchema);

        builder
            .HasOne(e => e.Delivery)
            .WithMany(e => e.Images);
    }
}
