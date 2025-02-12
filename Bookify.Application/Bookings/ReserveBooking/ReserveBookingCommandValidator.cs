
using FluentValidation;

namespace Bookify.Application.Bookings.ReserveBooking
{
    public class ReserveBookingCommandValidator : AbstractValidator<ReserveBookingCommand>
    {
        public ReserveBookingCommandValidator()
        {
            //Custom validation rules for the ReserveBookingCommand
            RuleFor(c => c.UserId).NotEmpty();
            RuleFor(c => c.AppartmentId).NotEmpty();
            RuleFor(c => c.StartDate).LessThan(c => c.EndDate);
        }
    }
}