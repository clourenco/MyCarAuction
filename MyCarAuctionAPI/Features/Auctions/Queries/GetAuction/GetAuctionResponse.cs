using MyCarAuction.Api.Features.Auctions.Common.Models;

namespace MyCarAuction.Api.Features.Auctions.Queries.GetAuction;

public sealed record GetAuctionResponse : BaseAuctionResponse
{
    public GetAuctionResponse(BaseAuctionResponse original) : base(original)
    {
    }

    public GetAuctionResponse(Guid id, bool isActive, string description) : base(id, isActive, description)
    {
    }
}
