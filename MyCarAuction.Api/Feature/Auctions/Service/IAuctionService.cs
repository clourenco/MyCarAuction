using MyCarAuction.Api.Domain.Models;

namespace MyCarAuction.Api.Feature.Auctions.Service;

internal interface IAuctionService
{
    Task<Auction> GetAuction(Guid id, CancellationToken cancellationToken);
    Task<Auction> StartAuction(Auction auction, Guid vehicleId, CancellationToken cancellationToken);
    Task<Auction> CloseAuction(Guid id, CancellationToken cancellationToken);
    Task<Bid> PlaceBid(Bid bid, CancellationToken cancellationToken);
}
