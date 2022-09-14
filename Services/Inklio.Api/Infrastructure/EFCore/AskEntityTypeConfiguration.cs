using Inklio.Api.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inklio.Api.Infrastructure.EFCore;

/// <summary>
/// Defines the EFCore enity configuration for an ask.
/// </summary>
class AskEntityTypeConfiguration : IEntityTypeConfiguration<Ask>
{
    public void Configure(EntityTypeBuilder<Ask> builder)
    {
        builder.ToTable("ask", InklioContext.DefaultDbSchema);

        builder.HasKey(o => o.Id);

        builder.Ignore(b => b.DomainEvents);

        builder.Property(o => o.Id)
            .UseHiLo("order_sequence", InklioContext.DefaultDbSchema);

        builder
            .HasMany(e => e.Comments)
            .WithOne();

        builder
            .HasMany(e => e.Deliveries)
            .WithOne();

        builder
            .Property<int>("createdById")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("created_by_id")
            .IsRequired();

        builder
            .Property<int?>("editedById")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("edited_by_id")
            .IsRequired(false);

        builder
            .Property<DateTime>("createdAtUtc")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("created_at_utc")
            .IsRequired();

        builder
            .Property<DateTime?>("editedAtUtc")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("edited_at_utc")
            .IsRequired(false);
    }
}
