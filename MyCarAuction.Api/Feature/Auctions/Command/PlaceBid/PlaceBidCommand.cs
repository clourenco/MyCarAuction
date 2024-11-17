using MediatR;

namespace MyCarAuction.Api.Feature.Auctions.Command.PlaceBid;

internal sealed record PlaceBidCommand(Guid AuctionId, Guid VehicleId, Guid BidderId, decimal Amount) : IRequest<PlaceBidResponse>;

