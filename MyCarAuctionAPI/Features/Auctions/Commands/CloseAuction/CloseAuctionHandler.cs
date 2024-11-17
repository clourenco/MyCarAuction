using MediatR;
using MyCarAuction.Api.Infrastructure.Services;

namespace MyCarAuction.Api.Features.Auctions.Commands.CloseAuction;

public sealed class CloseAuctionHandler : IRequestHandler<CloseAuctionCommand, CloseAuctionResponse>
{
    private readonly IAuctionService _auctionService;

    public CloseAuctionHandler(IAuctionService auctionService)
    {
        _auctionService = auctionService ?? throw new ArgumentNullException(nameof(auctionService));
    }

    public async Task<CloseAuctionResponse> Handle(CloseAuctionCommand request, CancellationToken cancellationToken)
    {
        var updatedAuction = await _auctionService.CloseAuction(request.Id, cancellationToken);

        return new CloseAuctionResponse(
            id: updatedAuction.Id,
            isActive: updatedAuction.IsActive,
            description: updatedAuction.Description
        );
    }
}
