using MyCarAuctionAPI.Infrastructure.Data.Entities;
using MyCarAuctionAPI.Infrastructure.Repository;

namespace MyCarAuction.Api.Features.Bids.Repository
{
    public interface IBidRepository : IBaseRepository<BidEntity>
    {
        Task<IEnumerable<BidEntity>> GetBidsByVehicleId(Guid vehicleId, CancellationToken cancellationToken);
    }
}
