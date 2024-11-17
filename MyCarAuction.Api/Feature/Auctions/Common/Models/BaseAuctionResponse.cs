namespace MyCarAuction.Api.Features.Auctions.Common.Models;

internal record BaseAuctionResponse(Guid Id, bool IsActive, string Description);
