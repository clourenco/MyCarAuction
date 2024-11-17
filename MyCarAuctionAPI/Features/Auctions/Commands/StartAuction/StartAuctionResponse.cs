using MyCarAuction.Api.Features.Auctions.Common.Models;

namespace MyCarAuction.Api.Features.Auctions.Commands.StartAuction;

public sealed record StartAuctionResponse : BaseAuctionResponse
{
    public StartAuctionResponse(BaseAuctionResponse original) : base(original)
    {
    }

    public StartAuctionResponse(Guid id, bool isActive, string description) : base(id, isActive, description)
    {
    }
}
