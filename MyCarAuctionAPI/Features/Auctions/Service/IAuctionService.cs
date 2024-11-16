using MyCarAuction.Api.Domain.Models;

namespace MyCarAuction.Api.Features.Auctions.Service
{
    public interface IAuctionService
    {
        Task<Auction> GetAuction(Guid id, CancellationToken cancellationToken);
        Task<Auction> StartAuction(Auction auction, Guid vehicleId, CancellationToken cancellationToken);
        Task<Auction> CloseAuction(Guid id, CancellationToken cancellationToken);
    }
}
