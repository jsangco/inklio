using Inklio.Api.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inklio.Api.Infrastructure.EFCore;

/// <summary>
/// Defines the EFCore enity configuration for an ask.
/// </summary>
class AskTagEntityTypeConfiguration : IEntityTypeConfiguration<AskTag>
{
    public void Configure(EntityTypeBuilder<AskTag> builder)
    {
        builder.ToTable("ask_tag", InklioContext.DefaultDbSchema);

        builder.HasKey(e => new { e.AskId, e.TagId });

        builder
            .HasOne(e => e.Ask)
            .WithMany("AskTags");

        builder
            .HasOne(e => e.Tag)
            .WithMany("AskTags");
    }
}
