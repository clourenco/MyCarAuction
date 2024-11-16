using MyCarAuction.Api.Domain.Models;

namespace MyCarAuction.Api.Features.Users.Service
{
    public interface IUserService
    {
        Task<User> GetUser(Guid id, CancellationToken cancellationToken);
        Task<User> AddUser(User user, CancellationToken cancellationToken);
    }
}
