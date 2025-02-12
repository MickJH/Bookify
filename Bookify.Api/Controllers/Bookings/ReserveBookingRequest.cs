namespace Bookify.Api.Controllers.Bookings
{
    public sealed record ReserveBookingRequest(
        Guid AppartmentId,
        Guid UserId,
        DateOnly StartDate,
        DateOnly EndDate);
    
}
