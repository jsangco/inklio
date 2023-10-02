using Inklio.Api.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inklio.Api.Infrastructure.EFCore;

/// <summary>
/// Defines the EFCore enity configuration for an <see cref="Comment"/>.
/// </summary>
class CommentEntityTypeConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("comment", InklioContext.DbSchema);

        // Indexes
        builder.HasKey(e => e.Id).IsClustered();
        builder.HasIndex(e => e.Id).IsUnique();

        // Gloabal query filters
        builder.HasQueryFilter(e => e.IsDeleted == false);

        // Ignored properties
        builder.Ignore(b => b.DomainEvents);
        builder.Ignore(b => b.IsUpvoted);

        // Relationships
        builder.HasOne(e => e.CreatedBy);
        builder.Navigation(e => e.CreatedBy).AutoInclude();

        builder
            .HasOne(e => e.Deletion)
            .WithOne(e => e.Comment);

        builder.HasOne(e => e.Thread);
        builder.Property<int>(e => e.ThreadId).IsRequired();

        builder
            .HasMany(e => e.Upvotes)
            .WithOne(e => e.Comment)
            .HasForeignKey(e => e.CommentId);

        builder
            .HasDiscriminator<byte>("CommentClassTypeId")
            .HasValue<Comment>((byte)CommentClassType.Comment)
            .HasValue<AskComment>((byte)CommentClassType.AskComment)
            .HasValue<DeliveryComment>((byte)CommentClassType.DeliveryComment);
    }
}
