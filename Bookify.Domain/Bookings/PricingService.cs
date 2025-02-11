
using Bookify.Domain.Appartments;
using Bookify.Domain.Bookings.Records;
using Bookify.Domain.Shared;

namespace Bookify.Domain.Bookings
{
    public class PricingService
    {
        public PricingDetails CalculatePrice(Appartment appartment, DateRange period)
        {
            var currency = appartment.Price.Currency;
            var priceForPeriod = new Money(appartment.Price.Amount * period.LengthInDays, currency);

            decimal percentageUpCharge = 0;
            foreach (var amenity in appartment.Amenities)
            {
                percentageUpCharge += amenity switch
                {
                    Amenity.GardenView or Amenity.MountainView => 0.05m,
                    Amenity.AirConditioning => 0.01m,
                    Amenity.Parking => 0.01m,
                    _ => 0
                };
            }

            var amenitiesUpCharge = Money.Zero(currency);
            if (percentageUpCharge > 0)
            {
                amenitiesUpCharge = new Money(priceForPeriod.Amount * percentageUpCharge, currency);
            }

            var totalPrice = Money.Zero();
            totalPrice += priceForPeriod;

            if (!appartment.CleaningFee.IsZero())
            {
                totalPrice += appartment.CleaningFee;
            }

            totalPrice += amenitiesUpCharge;
            return new PricingDetails(priceForPeriod, appartment.CleaningFee, amenitiesUpCharge, totalPrice);
        }
    }
}