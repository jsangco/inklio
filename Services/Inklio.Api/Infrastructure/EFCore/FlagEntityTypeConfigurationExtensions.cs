using Inklio.Api.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inklio.Api.Infrastructure.EFCore;

/// <summary>
/// Defines the EFCore enity configuration for an <see cref="Flag"/>.
/// </summary>
public static class FlagEntityTypeConfigurationExtensions
{
    public static ModelBuilder ApplyFlagEntityConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Flag>().ToTable("flag", InklioContext.DefaultDbSchema);
        builder.Entity<AskFlag>().ToTable("flag", InklioContext.DefaultDbSchema);
        builder.Entity<CommentFlag>().ToTable("flag", InklioContext.DefaultDbSchema);
        builder.Entity<DeliveryFlag>().ToTable("flag", InklioContext.DefaultDbSchema);

        builder.Entity<Flag>().HasKey(e => e.Id).IsClustered();
        builder.Entity<Flag>().HasIndex(e => e.Id).IsUnique();

        builder.Entity<AskFlag>().HasOne(e => e.Ask).WithMany(e => e.Flags);
        builder.Entity<CommentFlag>().HasOne(e => e.Comment).WithMany(e => e.Flags);
        builder.Entity<DeliveryFlag>().HasOne(e => e.Delivery).WithMany(e => e.Flags);

        builder.Entity<Flag>().Ignore(b => b.DomainEvents);
        builder.Entity<AskFlag>().Ignore(b => b.DomainEvents);
        builder.Entity<CommentFlag>().Ignore(b => b.DomainEvents);
        builder.Entity<DeliveryFlag>().Ignore(b => b.DomainEvents);

        builder.Entity<Flag>().HasOne(e => e.CreatedBy);

        builder.Entity<Flag>()
            .HasDiscriminator<byte>("FlagClassTypeId")
            .HasValue<Flag>((byte)FlagClassType.Flag)
            .HasValue<AskFlag>((byte)FlagClassType.AskFlag)
            .HasValue<DeliveryFlag>((byte)FlagClassType.DeliveryFlag)
            .HasValue<CommentFlag>((byte)FlagClassType.CommentFlag);

        return builder;
    }
}
