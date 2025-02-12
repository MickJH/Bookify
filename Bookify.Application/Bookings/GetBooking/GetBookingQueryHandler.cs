
using Bookify.Application.Abstractions.Data;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Abstractions;
using Dapper;

namespace Bookify.Application.Bookings.GetBooking
{
    internal sealed class GetBookingQueryHandler : IQueryHandler<GetBookingQuery, BookingResponse>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetBookingQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<BookingResponse>> Handle(GetBookingQuery query, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sqlQueryGetBooking = """
                SELECT
                    id AS Id,
                    user_id AS UserId,
                    appartment_id AS AppartmentId,
                    status AS Status,
                    price_for_period_amount AS PriceAmount,
                    price_for_period_currency AS PriceCurrency,
                    cleaning_fee_amount AS CleaningFeeAmount,
                    cleaning_fee_currency AS CleaningFeeCurrency,
                    amenities_up_charge_amount AS AmenitiesUpChargeAmount,
                    amenities_up_change_currency AS AmenitiesUpChargeCurrency,
                    total_price_amount AS TotalPriceAmount,
                    total_price_currency AS TotalPriceCurrency,
                    duration_start AS DurationStart,
                    duration_end AS DurationEnd,
                    created_on_utc AS CreatedOnUtc
                FROM Bookings
                WHERE Id = @BookingId
            """;

            var booking = await connection.QuerySingleOrDefaultAsync<BookingResponse>(sqlQueryGetBooking, new { query.BookingId });

            return booking;
        }


    }
}