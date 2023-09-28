using Inklio.Api.Domain;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inklio.Api.Infrastructure.EFCore;

/// <summary>
/// Defines the EFCore enity configuration for an <see cref="AskUpvote"/>.
/// </summary>
class AskUpvoteEntityTypeConfiguration : IEntityTypeConfiguration<AskUpvote>
{
    public void Configure(EntityTypeBuilder<AskUpvote> builder)
    {
        builder.ToTable("upvote", InklioContext.DbSchema);
    }
}
