using Microsoft.EntityFrameworkCore;
using MyCarAuctionAPI.Infrastructure.Data;
using MyCarAuctionAPI.Infrastructure.Data.Entities;
using MyCarAuctionAPI.Infrastructure.Repository;

namespace MyCarAuction.Api.Features.Users.Repository
{
    public sealed class UserRepository(IDbContextFactory<ApiDbContext> dbContextFactory) : BaseRepository<UserEntity>(dbContextFactory), IUserRepository
    {
    }
}
