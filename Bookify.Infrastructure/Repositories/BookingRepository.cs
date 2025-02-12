using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookify.Domain.Appartments;
using Bookify.Domain.Bookings;
using Bookify.Domain.Bookings.Records;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure.Repositories
{
    internal sealed class BookingRepository : Repository<Booking>, IBookingRepository
    {
        private static readonly BookingStatus[] ActiveBookingStatuses =
        {
            BookingStatus.Reserved,
            BookingStatus.Confirmed,
            BookingStatus.Completed
        };

        public BookingRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> IsOverlappingAsync(Appartment appartment, DateRange duration, CancellationToken cancellationToken = default)
        {
            return await DbContext.Set<Booking>()
                .AnyAsync(booking => booking.AppartmentId == appartment.Id && booking.Duration.Start <= duration.End
                && booking.Duration.End >= duration.Start && ActiveBookingStatuses.Contains(booking.Status), cancellationToken);
        }
    }
}
