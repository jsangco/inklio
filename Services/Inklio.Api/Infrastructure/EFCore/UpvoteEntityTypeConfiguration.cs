using Inklio.Api.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inklio.Api.Infrastructure.EFCore;

/// <summary>
/// Defines the EFCore enity configuration for an <see cref="Upvote"/>.
/// </summary>
class UpvoteEntityTypeConfiguration : IEntityTypeConfiguration<Upvote>
{
    public void Configure(EntityTypeBuilder<Upvote> builder)
    {
        builder.ToTable("upvote", InklioContext.DbSchema);

        builder.HasKey(e => e.Id).IsClustered();
        builder.HasIndex(e => e.Id).IsUnique();

        builder.Ignore(b => b.DomainEvents);

        // builder.Property(o => o.Id);
        // .UseHiLo("order_sequence", InklioContext.DefaultDbSchema);

        builder
            .HasDiscriminator<byte>("UpvoteClassTypeId")
            .HasValue<Upvote>((byte)UpvoteClassType.Upvote)
            .HasValue<AskUpvote>((byte)UpvoteClassType.AskUpvote)
            .HasValue<DeliveryUpvote>((byte)UpvoteClassType.DeliveryUpvote)
            .HasValue<CommentUpvote>((byte)UpvoteClassType.CommentUpvote);
    }
}
