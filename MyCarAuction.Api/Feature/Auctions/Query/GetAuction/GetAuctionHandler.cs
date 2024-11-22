﻿using MediatR;
using MyCarAuction.Api.Feature.Auctions.Service;

namespace MyCarAuction.Api.Features.Auctions.Queries.GetAuction;

internal sealed class GetAuctionHandler : IRequestHandler<GetAuctionQuery, GetAuctionResponse>
{
    private readonly IAuctionService _auctionService;

    public GetAuctionHandler(IAuctionService auctionService)
    {
        _auctionService = auctionService ?? throw new ArgumentNullException(nameof(auctionService));
    }

    public async Task<GetAuctionResponse> Handle(GetAuctionQuery request, CancellationToken cancellationToken)
    {
        var auction = await _auctionService.GetAuction(request.Id, cancellationToken);

        return new GetAuctionResponse(
            id: auction.Id,
            isActive: auction.IsActive,
            description: auction.Description
        );
    }
}
