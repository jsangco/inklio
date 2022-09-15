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
    }
}
