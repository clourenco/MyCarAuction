using MediatR;

namespace MyCarAuction.Api.Features.Bids.Commands.PlaceBid;

public sealed record PlaceBidCommand(Guid AuctionId, Guid VehicleId, Guid BidderId, decimal Amount) : IRequest<PlaceBidResponse>;

