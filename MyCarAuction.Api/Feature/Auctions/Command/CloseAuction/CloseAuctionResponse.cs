using MyCarAuction.Api.Features.Auctions.Common.Models;

namespace MyCarAuction.Api.Features.Auctions.Commands.CloseAuction;

internal sealed record CloseAuctionResponse : BaseAuctionResponse
{
    public CloseAuctionResponse(BaseAuctionResponse original) : base(original)
    {
    }

    public CloseAuctionResponse(Guid id, bool isActive, string description) : base(id, isActive, description)
    {
    }
}
