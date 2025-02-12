
using Bookify.Application.Abstractions.Data;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Appartments.SearchAppartments.Address;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Bookings;
using Dapper;

namespace Bookify.Application.Appartments.SearchAppartments
{
    internal sealed class SearchAppartmentQueryHandler : IQueryHandler<SearchAppartmentsQuery, IReadOnlyList<AppartmentResponse>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private static readonly int[] ActiveBookingStatuses =
        {
            (int)BookingStatus.Reserved,
            (int)BookingStatus.Confirmed,
            (int)BookingStatus.Completed
        };
        public SearchAppartmentQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<IReadOnlyList<AppartmentResponse>>> Handle(SearchAppartmentsQuery query, CancellationToken cancellationToken)
        {
            //Check if start date is not greater than end date
            if (query.StartDate > query.EndDate)
            {
                //Return emtpy list
                return new List<AppartmentResponse>();
            }

            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sqlQueryGetAppartments = """
                SELECT
                    a.id AS Id,
                    a.name AS Name,
                    a.description AS Description,
                    a.price_amount AS Price,
                    a.price_currency AS Currency,
                    a.address_country AS Country,
                    a.address_state AS State,
                    a.address_zip_code AS ZipCode,
                    a.address_city AS City,
                    a.address_street AS Street
                FROM Appartments AS a
                WHERE NOT EXISTS
                (
                    SELECT 1
                    FROM bookings AS b
                    WHERE
                        b.appartment_id = a.id AND
                        b.duration_start < @EndDate AND
                        b.duration_end > @StartDate AND
                        b.status = ANY(@ActiveBookingStatuses)
                )
            """;

            var appartments = await connection.QueryAsync<AppartmentResponse, AddressResponse, AppartmentResponse>(
                sqlQueryGetAppartments,
                (appartment, address) =>
                {
                    appartment.Address = address;
                    return appartment;
                },
                new
                {
                    query.StartDate,
                    query.EndDate,
                    ActiveBookingStatuses
                },
                splitOn: "Country");

            return appartments.ToList();
        }
    }
}