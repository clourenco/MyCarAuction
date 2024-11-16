using MyCarAuctionAPI.Infrastructure.Data.Entities;
using MyCarAuctionAPI.Infrastructure.Repository;

namespace MyCarAuction.Api.Features.Users.Repository
{
    public interface IUserRepository : IBaseRepository<UserEntity>
    {
    }
}
