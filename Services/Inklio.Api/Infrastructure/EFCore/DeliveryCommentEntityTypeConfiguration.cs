using Inklio.Api.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inklio.Api.Infrastructure.EFCore;

/// <summary>
/// Defines the EFCore enity configuration for an <see cref="DeliveryComment"/>.
/// </summary>
class DeliveryCommentEntityTypeConfiguration : IEntityTypeConfiguration<DeliveryComment>
{
    public void Configure(EntityTypeBuilder<DeliveryComment> builder)
    {
        builder.ToTable("comment", InklioContext.DefaultDbSchema);

        builder
            .HasOne(e => e.Delivery)
            .WithMany(e => e.Comments);
    }
}
