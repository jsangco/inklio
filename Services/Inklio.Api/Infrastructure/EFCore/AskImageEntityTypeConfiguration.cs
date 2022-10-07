using Inklio.Api.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inklio.Api.Infrastructure.EFCore;

/// <summary>
/// Defines the EFCore enity configuration for an <see cref="AskImage"/>.
/// </summary>
class AskImageEntityTypeConfiguration : IEntityTypeConfiguration<AskImage>
{
    public void Configure(EntityTypeBuilder<AskImage> builder)
    {
        builder.ToTable("image", InklioContext.DbSchema);

        builder
            .HasOne(e => e.Ask)
            .WithMany(e => e.Images);
    }
}
