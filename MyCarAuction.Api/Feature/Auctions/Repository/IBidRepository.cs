using MyCarAuction.Api.Infrastructure.Data.Entities;
using MyCarAuction.Api.Infrastructure.Repository;

namespace MyCarAuction.Api.Feature.Auctions.Repository
{
    internal interface IBidRepository : IBaseRepository<BidEntity>
    {
        Task<IEnumerable<BidEntity>> GetBidsByAuctionIdVehicleId(Guid AuctionId, Guid vehicleId, CancellationToken cancellationToken);
        Task<decimal> GetMaxAmountBidByAuctionIdVehicleId(Guid auctionId, Guid vehicleId, CancellationToken cancellationToken);
    }
}
