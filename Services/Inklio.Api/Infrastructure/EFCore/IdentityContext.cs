using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Inklio.Api.Infrastructure.EFCore;

public class IdentityContext : IdentityDbContext<InklioIdentityUser, IdentityRole<Guid>, Guid>, IDataProtectionKeyContext
{
    public static string DbSchema { get; private set; } = "auth";

    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

    public IdentityContext(DbContextOptions<IdentityContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema(DbSchema);

        // Custom model binding should be added after base.OnModelCreating
    }
}
