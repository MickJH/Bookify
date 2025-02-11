
namespace Bookify.Domain.Appartments.Records
{
   public record Address(
    string Country,
    string State,
    string ZipCode,
    string City,
    string Street
   );
}
