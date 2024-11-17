using MyCarAuction.Api.Infrastructure.Data.Entities;
using MyCarAuction.Api.Infrastructure.Repository;

namespace MyCarAuction.Api.Features.Users.Repository;

internal interface IUserRepository : IBaseRepository<UserEntity>
{
    Task<IEnumerable<UserEntity>> FindUser(string name, string email, CancellationToken cancellationToken);
}
