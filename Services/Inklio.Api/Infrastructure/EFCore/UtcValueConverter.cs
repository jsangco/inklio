using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Inklio.Api.Infrastructure.EFCore;

class UtcValueConverter : ValueConverter<DateTime, DateTime>
{
    public UtcValueConverter()
        : base(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
    {
    }
}