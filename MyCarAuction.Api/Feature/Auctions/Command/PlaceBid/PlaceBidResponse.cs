namespace MyCarAuction.Api.Feature.Auctions.Command.PlaceBid;

internal record PlaceBidResponse(Guid Id, Guid AuctionId, Guid VehicleId, Guid BidderId, decimal Amount);

