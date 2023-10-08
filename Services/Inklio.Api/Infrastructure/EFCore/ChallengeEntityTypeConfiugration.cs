using Inklio.Api.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json.Linq;

namespace Inklio.Api.Infrastructure.EFCore;

/// <summary>
/// Defines the EFCore enity configuration for an <see cref="Challenge"/>.
/// </summary>
class ChallengeEntityTypeConfiguration : IEntityTypeConfiguration<Challenge>
{
    public void Configure(EntityTypeBuilder<Challenge> builder)
    {
        builder.ToTable("challenge", InklioContext.DbSchema);

        // Indexes
        builder.HasKey(e => e.Id).IsClustered();
        builder.HasIndex(e => e.Id).IsUnique();

        // Ignores
        builder.Ignore(b => b.DomainEvents);

        // Properties
        builder.Property(e => e.ChallengeType).HasConversion<byte>().HasColumnName("challenge_type_id");
        builder.Property(e => e.State).HasConversion<byte>().HasColumnName("challenge_state_id");

        // Relationships
        builder.HasOne(e => e.CreatedBy);
        builder.Navigation(e => e.CreatedBy).AutoInclude();
    }
}
