using Inklio.Api.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inklio.Api.Infrastructure.EFCore;

/// <summary>
/// Defines the EFCore enity configuration for an <see cref="Image"/>.
/// </summary>
class ImageEntityTypeConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.ToTable("image", InklioContext.DbSchema);

        builder.HasKey(e => e.Id).IsClustered();
        builder.HasIndex(e => e.Id).IsUnique();

        builder.Ignore(b => b.DomainEvents);

        // builder.Property(o => o.Id);
            // .UseHiLo("order_sequence", InklioContext.DefaultDbSchema);

        builder.HasOne(e => e.Thread);
        builder.Property<int>(e => e.ThreadId).IsRequired();

        builder
            .HasDiscriminator<byte>("ImageClassTypeId")
            .HasValue<Image>((byte)ImageClassType.Image)
            .HasValue<AskImage>((byte)ImageClassType.AskImage)
            .HasValue<DeliveryImage>((byte)ImageClassType.DeliveryImage);
    }
}
