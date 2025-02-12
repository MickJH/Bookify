
using Bookify.Application.Abstractions.Clock;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Appartments;
using Bookify.Domain.Bookings;
using Bookify.Domain.Bookings.Records;
using Bookify.Domain.Users;
using static Bookify.Application.Abstractions.Messaging.ICommandHandler;


namespace Bookify.Application.Bookings.ReserveBooking
{
    internal sealed class ReserveBookingCommandHandler : ICommandHandler<ReserveBookingCommand, Guid> {

        private readonly IUserRepository _userRepository;
        private readonly IAppartmentRepository _appartmentRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly PricingService _pricingService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public ReserveBookingCommandHandler (IUserRepository userRepository, IAppartmentRepository appartmentRepository, IBookingRepository bookingRepository, 
            IUnitOfWork unitOfWork, PricingService pricingService, IDateTimeProvider dateTimeProvider)
        {
            _userRepository = userRepository;
            _appartmentRepository = appartmentRepository;
            _bookingRepository = bookingRepository;
            _unitOfWork = unitOfWork;
            _pricingService = pricingService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Result<Guid>> Handle(ReserveBookingCommand request, CancellationToken cancellationToken)
        {
            // Get user by Id
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

            if (user == null)
            {
                return Result.Failure<Guid>(UserErrors.NotFound);
            }

            //Get Appartment by Id  
            var appartment = await _appartmentRepository.GetByIdAsync(request.UserId, cancellationToken);

            if (appartment == null)
            {
                return Result.Failure<Guid>(BookingErrors.NotFound);
            }

            // Create DateRange, it is not null since it's validated in the record   
            var duration = DateRange.Create(request.StartDate, request.EndDate);

            // Check to see if booking is overlapping another booking
             if (await _bookingRepository.IsOverlappingAsync(appartment, duration, cancellationToken))
            {
                return Result.Failure<Guid>(BookingErrors.Overlap);
            }

            // Reserve booking
            try
            {
                var booking = Booking.Reserve(appartment, user.Id, duration, _dateTimeProvider.UtcNow, _pricingService);

                _bookingRepository.Add(booking);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return booking.Id;
            } catch (Exception ex)
            {
                return Result.Failure<Guid>(BookingErrors.Overlap);
            }
        }
    }   
     
}