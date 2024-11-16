using MyCarAuction.Api.Domain.Models;

namespace MyCarAuction.Api.Features.Bids.Service
{
    public interface IBidService
    {
        Task<Bid> PlaceBid(Bid bid, CancellationToken cancellationToken);
        Task<IEnumerable<Bid>> GetBidsByVehicle(Guid vehicleId, CancellationToken cancellationToken);
    }
}
