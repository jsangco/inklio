using Inklio.Api.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inklio.Api.Infrastructure.EFCore;

/// <summary>
/// Defines the EFCore enity configuration for an <see cref="LockInfo"/>.
/// </summary>
class LockInfoEntityTypeConfiguration : IEntityTypeConfiguration<LockInfo>
{
    public void Configure(EntityTypeBuilder<LockInfo> builder)
    {
        builder.ToTable("lock_info", InklioContext.DbSchema);

        // Indexes
        builder.HasKey(e => e.Id).IsClustered();
        builder.HasIndex(e => e.Id).IsUnique();

        // Ignores
        builder.Ignore(b => b.DomainEvents);

        builder.Property(e => e.LockType).HasConversion<byte>();
    }
}
