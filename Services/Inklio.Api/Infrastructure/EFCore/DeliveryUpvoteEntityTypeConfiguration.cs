using Inklio.Api.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inklio.Api.Infrastructure.EFCore;

/// <summary>
/// Defines the EFCore enity configuration for an <see cref="DeliveryUpvote"/>.
/// </summary>
class DeliveryUpvoteEntityTypeConfiguration : IEntityTypeConfiguration<DeliveryUpvote>
{
    public void Configure(EntityTypeBuilder<DeliveryUpvote> builder)
    {
        builder.ToTable("upvote", InklioContext.DbSchema);

        builder
            .HasOne(e => e.Delivery)
            .WithMany(e => e.Upvotes);
    }
}
