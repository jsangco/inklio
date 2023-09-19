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

        builder.HasKey(e => e.Id).IsClustered();
        builder.HasIndex(e => e.Id).IsUnique();

        builder.Ignore(b => b.DomainEvents);

        builder.HasOne(e => e.CreatedBy);
        builder.Navigation(e => e.CreatedBy).AutoInclude();

        // builder.Property(o => o.Id);
            // .UseHiLo("order_sequence", InklioContext.DefaultDbSchema);

        builder.HasOne(e => e.Thread);
        builder.Property<int>(e => e.ThreadId).IsRequired();

        builder
            .HasDiscriminator<byte>("CommentClassTypeId")
            .HasValue<Comment>((byte)CommentClassType.Comment)
            .HasValue<AskComment>((byte)CommentClassType.AskComment)
            .HasValue<DeliveryComment>((byte)CommentClassType.DeliveryComment);
    }
}
