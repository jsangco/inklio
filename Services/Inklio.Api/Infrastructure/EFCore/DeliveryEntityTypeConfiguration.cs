using Inklio.Api.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inklio.Api.Infrastructure.EFCore;

/// <summary>
/// Defines the EFCore enity configuration for an ask.
/// </summary>
class DeliveryEntityTypeConfiguration : IEntityTypeConfiguration<Delivery>
{
    public void Configure(EntityTypeBuilder<Delivery> builder)
    {
        builder.ToTable("delivery", InklioContext.DefaultDbSchema);

        builder.HasKey(o => o.Id);

        builder.Ignore(b => b.DomainEvents);

        builder.Property(o => o.Id)
            .UseHiLo("order_sequence", InklioContext.DefaultDbSchema);

        builder
            .HasOne(e => e.Ask)
            .WithMany(e => e.Deliveries);

        builder
            .HasMany(e => e.Comments)
            .WithOne();
    }
}
