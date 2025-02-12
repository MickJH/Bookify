using Bookify.Domain.Abstractions;
using Bookify.Domain.Appartments.Records;
using Bookify.Domain.Shared;

namespace Bookify.Domain.Appartments
{
    public sealed class Appartment : Entity
    {
        // Empty constructor for Databse Migration
        private Appartment() { }
        public Appartment(Guid id, Name name, Description description, Address address, Money price, Money cleaningFee, List<Amenity> amenities) : base(id)
        {
            Name = name;
            Description = description;
            Address = address;
            Price = price;
            CleaningFee = cleaningFee;
            Amenities = amenities;
        }
        public Name Name { get; private set; }
        public Description Description { get; private set; }
        public Address Address { get; private set; }
        public Money Price { get; private set; }
        public Money CleaningFee { get; private set; }
        public DateTime? LastBookedOnUtc { get; internal set; }
        public List<Amenity> Amenities { get; private set; } = new();
    }
}
