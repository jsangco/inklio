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
        builder.ToTable("delivery", InklioContext.DbSchema);

        builder.HasKey(e => e.Id).IsClustered();
        builder.HasIndex(e => e.Id).IsUnique();

        builder.Ignore(b => b.DomainEvents);

        builder
            .HasOne(e => e.Ask)
            .WithMany(e => e.Deliveries);

        builder
            .HasMany(e => e.Comments)
            .WithOne();

        builder
            .HasMany(e => e.Images)
            .WithOne();
    
        builder
            .HasMany(e => e.Tags)
            .WithMany(e => e.Deliveries)
            .UsingEntity<DeliveryTag>(
                e => e
                    .HasOne(dt => dt.Tag)
                    .WithMany(t => t.DeliveryTags)
                    .HasForeignKey(dt => dt.TagId),
                e => e
                    .HasOne(dt => dt.Delivery)
                    .WithMany(a => a.DeliveryTags)
                    .HasForeignKey(dt => dt.DeliveryId),
                e =>
                {
                    e.Property(f => f.CreatedAtUtc);
                    e.HasOne(f => f.CreatedBy);
                    e.HasKey(dt => new { dt.DeliveryId, dt.TagId });
                    e.HasIndex(dt => new { dt.DeliveryId, dt.TagId }).IsUnique();
                });
        
    }
}
