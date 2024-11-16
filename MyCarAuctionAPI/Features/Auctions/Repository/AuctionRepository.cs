using Microsoft.EntityFrameworkCore;
using MyCarAuctionAPI.Infrastructure.Data;
using MyCarAuctionAPI.Infrastructure.Data.Entities;
using MyCarAuctionAPI.Infrastructure.Repository;

namespace MyCarAuction.Api.Features.Auctions.Repository
{
    public sealed class AuctionRepository(IDbContextFactory<ApiDbContext> dbContextFactory) : BaseRepository<AuctionEntity>(dbContextFactory), IAuctionRepository
    {
    }
}
