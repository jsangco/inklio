using Inklio.Api.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inklio.Api.Infrastructure.EFCore;

/// <summary>
/// Defines the EFCore enity configuration for an <see cref="Deletion"/>.
/// </summary>
class DeletionEntityTypeConfiguration : IEntityTypeConfiguration<Deletion>
{
    public void Configure(EntityTypeBuilder<Deletion> builder)
    {
        builder.ToTable("deletion", InklioContext.DbSchema);

        // Indexes
        builder.HasKey(e => e.Id).IsClustered();
        builder.HasIndex(e => e.Id).IsUnique();

        // Ignores
        builder.Ignore(b => b.DomainEvents);

        // Property mappings
        builder.Property(e => e.DeletionType).HasConversion<int>().HasColumnName("deletion_type_id");
        builder.Property(e => e.InternalComment).HasColumnName("internal_comment");
        builder.Property(e => e.UserMessage).HasColumnName("user_message");

        // Discriminator
        builder
            .HasDiscriminator<byte>("DeletionClassTypeId")
            .HasValue<Deletion>((byte)DeletionClassType.Deletion)
            .HasValue<AskDeletion>((byte)DeletionClassType.AskDeletion)
            .HasValue<DeliveryDeletion>((byte)DeletionClassType.DeliveryDeletion)
            .HasValue<CommentDeletion>((byte)DeletionClassType.CommentDeletion);
    }
}
