
using System.Dynamic;
using System.Net.NetworkInformation;
using System.Reflection.Metadata.Ecma335;
using System.Xml.XPath;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Appartments;
using Bookify.Domain.Bookings.Events;
using Bookify.Domain.Bookings.Records;
using Bookify.Domain.Shared;

namespace Bookify.Domain.Bookings
{
    public sealed class Booking : Entity
    {
        // Empty constructor for Database Migration
        private Booking() { }
        private Booking(Guid id, Guid appartmentId, Guid userId, DateRange duration, Money priceForPeriod, Money cleaningFee, Money amenitiesUpCharge, Money totalPrice,
        BookingStatus status, DateTime createdOnUtc) : base(id)
        {
            AppartmentId = appartmentId;
            UserId = userId;
            Duration = duration;
            PriceForPeriod = priceForPeriod;
            CleaningFee = cleaningFee;
            AmenitiesUpCharge = amenitiesUpCharge;
            TotalPrice = totalPrice;
            Status = status;
            CreatedOnUtc = createdOnUtc;
        }

        public Guid AppartmentId { get; private set; }
        public Guid UserId { get; private set; }
        public DateRange Duration { get; private set; }
        public Money PriceForPeriod { get; private set; }
        public Money CleaningFee { get; private set; }
        public Money AmenitiesUpCharge { get; private set; }
        public Money TotalPrice { get; private set; }
        public BookingStatus Status { get; private set; }
        public DateTime CreatedOnUtc { get; private set; }
        public DateTime? ConfirmedOnUtc { get; private set; }
        public DateTime? RejectedOnUtc { get; private set; }
        public DateTime? CompletedOnUtc { get; private set; }
        public DateTime? CancelledOnUtc { get; private set; }

        public static Booking Reserve(Appartment appartment, Guid userId, DateRange duration, DateTime utcNow, PricingService pricingService)
        {
            var pricingDetails = pricingService.CalculatePrice(appartment, duration);

            var booking = new Booking(Guid.NewGuid(), appartment.Id, userId, duration, pricingDetails.PriceForPeriod, pricingDetails.CleaningFee, pricingDetails.AmenitiesUpCharge, pricingDetails.TotalPrice, BookingStatus.Reserved, utcNow);

            booking.RaiseDomainEvent(new BookingReservedDomainEvent(booking.Id));

            appartment.LastBookedOnUtc = utcNow;

            return booking;
        }

        public Result Confirm(DateTime utcNow)
        {
            if (Status != BookingStatus.Reserved)
            {
                return Result.Failure(BookingErrors.NotReserved);
            }

            Status = BookingStatus.Confirmed;
            ConfirmedOnUtc = utcNow;

            RaiseDomainEvent(new BookingConfirmedDomainEvent(Id));

            return Result.Success();
        }

        public Result Reject(DateTime utcNow)
        {
            if (Status != BookingStatus.Reserved)
            {
                return Result.Failure(BookingErrors.NotReserved);
            }

            Status = BookingStatus.Rejected;
            RejectedOnUtc = utcNow;

            RaiseDomainEvent(new BookingRejectedDomainEvent(Id));

            return Result.Success();
        }

        public Result Complete(DateTime utcNow)
        {
            if (Status != BookingStatus.Confirmed)
            {
                return Result.Failure(BookingErrors.NotConfirmed);
            }

            Status = BookingStatus.Completed;
            CompletedOnUtc = utcNow;

            RaiseDomainEvent(new BookingCompletedDomainEvent(Id));

            return Result.Success();
        }

        public Result Cancel(DateTime utcNow)
        {
            if (Status != BookingStatus.Confirmed)
            {
                return Result.Failure(BookingErrors.NotConfirmed);
            }

            var currentDate = DateOnly.FromDateTime(utcNow);

            if (currentDate > Duration.Start)
            {
                return Result.Failure(BookingErrors.AlreadyStarted);
            }

            Status = BookingStatus.Cancelled;
            CancelledOnUtc = utcNow;

            RaiseDomainEvent(new BookingCancelledDomainEvent(Id));

            return Result.Success();
        }
    }
}