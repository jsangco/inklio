using Inklio.Api.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json.Linq;

namespace Inklio.Api.Infrastructure.EFCore;

/// <summary>
/// Defines the EFCore enity configuration for an <see cref="Challenge"/>.
/// </summary>
class ChallengeDeliveryRankEntityTypeConfiguration : IEntityTypeConfiguration<ChallengeDeliveryRank>
{
    public void Configure(EntityTypeBuilder<ChallengeDeliveryRank> builder)
    {
        builder.ToTable("challenge_delivery_rank", InklioContext.DbSchema);

        // Indexes
        builder.HasKey(e => e.Id).IsClustered();
        builder.HasIndex(e => e.Id).IsUnique();

        // Ignores
        builder.Ignore(b => b.DomainEvents);

        // Relationships
        builder.HasOne(e => e.Ask);
        builder.HasOne(e => e.Delivery);
        builder.HasOne(e => e.Challenge);
    }
}
