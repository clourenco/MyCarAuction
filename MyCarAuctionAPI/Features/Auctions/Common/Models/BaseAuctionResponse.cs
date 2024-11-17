namespace MyCarAuction.Api.Features.Auctions.Common.Models;

public record BaseAuctionResponse(Guid Id, bool IsActive, string Description);
