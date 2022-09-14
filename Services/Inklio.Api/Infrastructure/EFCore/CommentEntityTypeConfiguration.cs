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
        builder.ToTable("Comment", InklioContext.DefaultDbSchema);

        builder.HasKey(o => o.Id);

        builder.Ignore(b => b.DomainEvents);

        builder.Property(o => o.Id)
            .UseHiLo("orderseq", InklioContext.DefaultDbSchema);

        builder.HasOne(e => e.Thread);

        builder
            .HasDiscriminator<byte>("CommentTypeId")
            .HasValue<Comment>((byte)CommentType.Comment)
            .HasValue<AskComment>((byte)CommentType.AskComment)
            .HasValue<DeliveryComment>((byte)CommentType.DeliveryComment);

        builder
            .Property<int>("createdById")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("createdById")
            .IsRequired();

        builder
            .Property<int?>("editedById")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("editedById")
            .IsRequired(false);

        builder
            .Property<DateTimeOffset>("createdAtUtc")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("createdAtUtc")
            .IsRequired();

        builder
            .Property<DateTimeOffset?>("editedAtUtc")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("editedAtUtc")
            .IsRequired(false);
    }
}
