namespace MyCarAuction.Api.Features.Bids.Commands.PlaceBid;

public sealed record PlaceBidResponse(Guid Id, Guid AuctionId, Guid VehicleId, Guid BidderId, decimal Amount);

