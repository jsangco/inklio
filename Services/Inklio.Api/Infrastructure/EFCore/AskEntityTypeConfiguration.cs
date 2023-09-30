using Inklio.Api.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inklio.Api.Infrastructure.EFCore;

/// <summary>
/// Defines the EFCore enity configuration for an ask.
/// </summary>
class AskEntityTypeConfiguration : IEntityTypeConfiguration<Ask>
{
    public void Configure(EntityTypeBuilder<Ask> builder)
    {
        builder.ToTable("ask", InklioContext.DbSchema);

        builder.HasKey(e => e.Id).IsClustered();
        builder.HasIndex(e => e.Id).IsUnique();

        builder.Ignore(b => b.DomainEvents);
        builder.Ignore(b => b.IsUpvoted);

        builder.HasOne(e => e.CreatedBy);
        builder.Navigation(e => e.CreatedBy).AutoInclude();

        builder
            .HasMany(e => e.Comments)
            .WithOne();

        builder
            .HasMany(e => e.Deliveries)
            .WithOne();

        builder
            .HasMany(e => e.Images)
            .WithOne();

        builder
            .HasMany(e => e.Upvotes)
            .WithOne(e => e.Ask)
            .HasForeignKey(e => e.AskId);

        builder
            .HasMany(e => e.Tags)
            .WithMany(e => e.Asks)
            .UsingEntity<AskTag>(
                e => e
                    .HasOne(at => at.Tag)
                    .WithMany(t => t.AskTags)
                    .HasForeignKey(at => at.TagId),
                e => e
                    .HasOne(at => at.Ask)
                    .WithMany(a => a.AskTags)
                    .HasForeignKey(at => at.AskId),
                e =>
                {
                    e.Property(f => f.CreatedAtUtc);
                    e.HasOne(f => f.CreatedBy);
                    e.HasKey(at => new { at.AskId, at.TagId });
                    e.HasIndex(at => new { at.AskId, at.TagId }).IsUnique();
                });
    }
}
