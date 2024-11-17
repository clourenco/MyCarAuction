using MediatR;
using MyCarAuction.Api.Domain.Models;
using MyCarAuction.Api.Infrastructure.Services;

namespace MyCarAuction.Api.Features.Bids.Commands.PlaceBid;

public sealed class PlaceBidHandler : IRequestHandler<PlaceBidCommand, PlaceBidResponse>
{
    private readonly IAuctionService _bidService;

    public PlaceBidHandler(IAuctionService bidService)
    {
        _bidService = bidService ?? throw new ArgumentNullException(nameof(bidService));
    }

    public async Task<PlaceBidResponse> Handle(PlaceBidCommand request, CancellationToken cancellationToken)
    {
        Bid bid = new Bid(
            id: Guid.NewGuid(),
            auctionId: request.AuctionId,
            vehicleId: request.VehicleId,
            bidderId: request.BidderId,
            amount: request.Amount
        );

        var newBid = await _bidService.PlaceBid(bid, cancellationToken);

        return new PlaceBidResponse(
            Id: newBid.Id,
            AuctionId: newBid.AuctionId,
            VehicleId: newBid.VehicleId,
            BidderId: newBid.BidderId,
            Amount: newBid.Amount
        );
    }
}
