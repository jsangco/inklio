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
        builder.ToTable("user", InklioContext.DbSchema);

        builder.HasKey(e => e.Id).IsClustered();
        builder.HasIndex(e => e.Id).IsUnique();
        builder.HasIndex(e => e.Username).IsUnique();

        builder.Ignore(b => b.DomainEvents);

        // builder.Property(o => o.Id).IsRequired();
            // .UseHiLo("order_sequence", InklioContext.DefaultDbSchema);

        builder.HasMany(e => e.Comments).WithOne(e => e.CreatedBy);
        builder.HasMany(e => e.Asks).WithOne(e => e.CreatedBy);
        builder.HasMany(e => e.Comments).WithOne(e => e.CreatedBy);
        builder.HasMany(e => e.Deliveries).WithOne(e =>e.CreatedBy);
        builder.HasMany(e => e.AskTags).WithOne(e => e.CreatedBy);
        builder.HasMany(e => e.DeliveryTags).WithOne(e => e.CreatedBy);
        builder.HasMany(e => e.AskUpvotes).WithOne(e => e.CreatedBy);
        builder.HasMany(e => e.CommentUpvotes).WithOne(e => e.CreatedBy);
        builder.HasMany(e => e.DeliveryUpvotes).WithOne(e => e.CreatedBy);
    }
}
