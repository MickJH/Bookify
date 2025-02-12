
using Bookify.Application.Abstractions.Messaging;

namespace Bookify.Application.Appartments.SearchAppartments
{
    public sealed record SearchAppartmentsQuery(DateOnly StartDate, DateOnly EndDate) : IQuery<IReadOnlyList<AppartmentResponse>>;
}