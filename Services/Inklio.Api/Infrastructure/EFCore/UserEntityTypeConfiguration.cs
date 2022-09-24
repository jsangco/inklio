using Inklio.Api.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inklio.Api.Infrastructure.EFCore;

/// <summary>
/// Defines the EFCore enity configuration for an <see cref="User"/>.
/// </summary>
class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("user", InklioContext.DefaultDbSchema);

        builder.HasKey(e => e.Id).IsClustered();
        builder.HasIndex(e => e.Id).IsUnique();
        builder.HasIndex(e => e.Username).IsUnique();

        builder.Ignore(b => b.DomainEvents);

        // builder.Property(o => o.Id).IsRequired();
            // .UseHiLo("order_sequence", InklioContext.DefaultDbSchema);

        builder
            .HasMany(e => e.Comments)
            .WithOne(e => e.CreatedBy);

        builder
            .HasMany(e => e.Asks)
            .WithOne(e => e.CreatedBy);

        builder
            .HasMany(e => e.Comments)
            .WithOne(e => e.CreatedBy);

        builder
            .HasMany(e => e.Deliveries)
            .WithOne(e =>e.CreatedBy);
    }
}
