using MyCarAuctionAPI.Infrastructure.Data.Entities;
using MyCarAuctionAPI.Infrastructure.Repository;

namespace MyCarAuction.Api.Features.Auctions.Repository
{
    public interface IAuctionRepository : IBaseRepository<AuctionEntity>
    {
    }
}
