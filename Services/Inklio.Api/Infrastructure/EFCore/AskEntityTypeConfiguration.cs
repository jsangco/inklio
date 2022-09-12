using Inklio.Api.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inklio.Api.Infrastructure.Sql;

class AskEntityTypeConfiguration : IEntityTypeConfiguration<Order>

{
    public void Configure(EntityTypeBuilder<Ask> askConfiguration)
    {
        askConfiguration.ToTable("ask", InklioContext.DefaultDbSchema);

        askConfiguration.HasKey(o => o.Id);

        askConfiguration.Ignore(b => b.DomainEvents);

        askConfiguration.Property(o => o.Id)
            .UseHiLo("orderseq", InklioContext.DefaultDbSchema);

        //Address value object persisted as owned entity type supported since EF Core 2.0
        askConfiguration
            .OwnsOne(o => o.Address, a =>
            {
                // Explicit configuration of the shadow key property in the owned type 
                // as a workaround for a documented issue in EF Core 5: https://github.com/dotnet/efcore/issues/20740
                a.Property<int>("OrderId")
                .UseHiLo("orderseq", OrderingContext.DEFAULT_SCHEMA);
                a.WithOwner();
            });

        askConfiguration
            .Property<int?>("_buyerId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("BuyerId")
            .IsRequired(false);

        askConfiguration
            .Property<DateTime>("_orderDate")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("OrderDate")
            .IsRequired();

        askConfiguration
            .Property<int>("_orderStatusId")
            // .HasField("_orderStatusId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("OrderStatusId")
            .IsRequired();

        askConfiguration
            .Property<int?>("_paymentMethodId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("PaymentMethodId")
            .IsRequired(false);

        askConfiguration.Property<string>("Description").IsRequired(false);

        var navigation = askConfiguration.Metadata.FindNavigation(nameof(Order.OrderItems));

        // DDD Patterns comment:
        //Set as field (New since EF 1.1) to access the OrderItem collection property through its field
        navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

        askConfiguration.HasOne<PaymentMethod>()
            .WithMany()
            // .HasForeignKey("PaymentMethodId")
            .HasForeignKey("_paymentMethodId")
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        askConfiguration.HasOne<Buyer>()
            .WithMany()
            .IsRequired(false)
            // .HasForeignKey("BuyerId");
            .HasForeignKey("_buyerId");

        askConfiguration.HasOne(o => o.OrderStatus)
            .WithMany()
            // .HasForeignKey("OrderStatusId");
            .HasForeignKey("_orderStatusId");
    }
}
