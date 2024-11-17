using MyCarAuctionAPI.Infrastructure.Data.Entities;
using MyCarAuctionAPI.Infrastructure.Repository;

namespace MyCarAuction.Api.Features.Bids.Repository
{
    public interface IBidRepository : IBaseRepository<BidEntity>
    {
        Task<IEnumerable<BidEntity>> GetBidsByAuctionIdVehicleId(Guid AuctionId, Guid vehicleId, CancellationToken cancellationToken);
        Task<decimal> GetMaxAmountBidByAuctionIdVehicleId(Guid auctionId, Guid vehicleId, CancellationToken cancellationToken);
    }
}
