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

        // builder.Property(e => e.Id);
            // .UseHiLo("order_sequence", InklioContext.DefaultDbSchema);

        builder.Ignore(e => e.DomainEvents);
        
        // builder
        //     .HasMany(e => e.Asks)
        //     .WithMany(e => e.Tags)
        //     .UsingEntity(e => e.ToTable("ask_tag"));

        // builder.HasMany("AskTags");
        // builder.HasMany(e => e.AskTags).WithOne(e => e.Tag);
    }
}
