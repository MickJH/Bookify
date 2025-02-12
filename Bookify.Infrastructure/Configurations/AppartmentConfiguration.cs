using Bookify.Domain.Appartments;
using Bookify.Domain.Appartments.Records;
using Bookify.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Configurations
{
    internal sealed class AppartmentConfiguration : IEntityTypeConfiguration<Appartment>
    {
        public void Configure(EntityTypeBuilder<Appartment> builder)
        {
            builder.ToTable("appartments");
            builder.HasKey(appartment => appartment.Id);
            builder.OwnsOne(appartment => appartment.Address);
            builder.Property(appartment => appartment.Name).HasMaxLength(200).HasConversion(name => name.Value, value => new Name(value));
            builder.Property(appartment => appartment.Description).HasMaxLength(2000).HasConversion(description => description.Value, value => new Description(value));
            builder.OwnsOne(appartment => appartment.Price, priceBuilder =>
            {
                priceBuilder.Property(money => money.Currency).HasConversion(currency => currency.Code, code => Currency.FromCode(code));
            });
            builder.OwnsOne(appartment => appartment.CleaningFee, priceBuilder =>
            {
                priceBuilder.Property(money => money.Currency).HasConversion(currency => currency.Code, code => Currency.FromCode(code));
            });
            builder.Property<uint>("Version").IsRowVersion();
        }
    }
}
