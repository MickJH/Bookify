
namespace Bookify.Domain.Appartments
{
    public interface IAppartmentRepository
    {
        Task<Appartment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}