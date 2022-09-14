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

        builder
            .Property<int>("createdById")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("createdById")
            .IsRequired();

        builder
            .Property<int?>("editedById")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("editedById")
            .IsRequired(false);

        builder
            .Property<DateTime>("createdAtUtc")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("createdAtUtc")
            .IsRequired();

        builder
            .Property<DateTime?>("editedAtUtc")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("editedAtUtc")
            .IsRequired(false);
    }
}
