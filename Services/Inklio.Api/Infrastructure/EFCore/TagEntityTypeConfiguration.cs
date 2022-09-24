using Inklio.Api.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inklio.Api.Infrastructure.EFCore;

/// <summary>
/// Defines the EFCore enity configuration for an <see cref="Tag"/>.
/// </summary>
class TagEntityTypeConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("tag", InklioContext.DefaultDbSchema);

        builder.HasKey(e => e.Id).IsClustered();
        builder.HasIndex(e => e.Id).IsUnique();

        builder.Ignore(e => e.DomainEvents);
    }
}
