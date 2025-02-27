﻿using Bookify.Domain.Abstractions;
using Bookify.Domain.Bookings;
using Bookify.Domain.Reviews.Events;

namespace Bookify.Domain.Reviews
{
    public sealed class Review : Entity
    {
        private Review(
            Guid id,
            Guid appartmentId,
            Guid bookingId,
            Guid userId,
            Rating rating,
            Comment comment,
            DateTime createdOnUtc)
            : base(id)
        {
            AppartmentId = appartmentId;
            BookingId = bookingId;
            UserId = userId;
            Rating = rating;
            Comment = comment;
            CreatedOnUtc = createdOnUtc;
        }

        private Review()
        {
        }

        public Guid AppartmentId { get; private set; }

        public Guid BookingId { get; private set; }

        public Guid UserId { get; private set; }

        public Rating Rating { get; private set; }

        public Comment Comment { get; private set; }

        public DateTime CreatedOnUtc { get; private set; }

        public static Result<Review> Create(
            Booking booking,
            Rating rating,
            Comment comment,
            DateTime createdOnUtc)
        {
            if (booking.Status != BookingStatus.Completed)
            {
                return Result.Failure<Review>(ReviewErrors.NotEligible);
            }

            var review = new Review(
                Guid.NewGuid(),
                booking.AppartmentId,
                booking.Id,
                booking.UserId,
                rating,
                comment,
                createdOnUtc);

            review.RaiseDomainEvent(new ReviewCreatedDomainEvent(review.Id));

            return review;
        }
    }
}

