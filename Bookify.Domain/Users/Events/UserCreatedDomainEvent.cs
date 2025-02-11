
using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Users.Records
{
    public sealed record UserCreatedDomainEvent(Guid UserId) : IDomainEvent;
}