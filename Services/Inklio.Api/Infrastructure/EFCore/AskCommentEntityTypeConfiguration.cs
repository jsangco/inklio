using Inklio.Api.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inklio.Api.Infrastructure.EFCore;

/// <summary>
/// Defines the EFCore enity configuration for an <see cref="AskComment"/>.
/// </summary>
class AskCommentEntityTypeConfiguration : IEntityTypeConfiguration<AskComment>
{
    public void Configure(EntityTypeBuilder<AskComment> builder)
    {
        builder.ToTable("comment", InklioContext.DefaultDbSchema);

        builder
            .HasOne(e => e.Ask)
            .WithMany(e => e.Comments);
    }
}
