
using Bookify.Application.Appartments.SearchAppartments.Address;

namespace Bookify.Application.Appartments.SearchAppartments
{
    public sealed class AppartmentResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public decimal Price { get; init; }
        public string Currency { get; init; }
        //Nested object
        public AddressResponse Address { get; set; }
    }
}