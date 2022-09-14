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
        builder.ToTable("AskComment", InklioContext.DefaultDbSchema);

        builder.HasKey(o => o.Id);

        builder.Ignore(b => b.DomainEvents);

        builder.Property(o => o.Id)
            .UseHiLo("orderseq", InklioContext.DefaultDbSchema);

        builder.Property<int>("AskId").IsRequired();

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
