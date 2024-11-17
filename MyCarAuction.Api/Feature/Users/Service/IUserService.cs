using MyCarAuction.Api.Domain.Models;

namespace MyCarAuction.Api.Features.Users.Service;

internal interface IUserService
{
    Task<User> GetUser(Guid id, CancellationToken cancellationToken);
    Task<User> AddUser(User user, CancellationToken cancellationToken);
    Task<IEnumerable<User>> SearchUser(string name, string email, CancellationToken cancellationToken);
}
