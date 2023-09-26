using Inklio.Api.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inklio.Api.Infrastructure.EFCore;

/// <summary>
/// Defines the EFCore enity configuration for an <see cref="CommentUpvote"/>.
/// </summary>
class CommentUpvoteEntityTypeConfiguration : IEntityTypeConfiguration<CommentUpvote>
{
    public void Configure(EntityTypeBuilder<CommentUpvote> builder)
    {
        builder.ToTable("upvote", InklioContext.DbSchema);
    }
}
