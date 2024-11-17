using MediatR;
using MyCarAuction.Api.Domain.Models;
using MyCarAuction.Api.Infrastructure.Services;

namespace MyCarAuction.Api.Features.Auctions.Commands.StartAuction
{
    public sealed class StartAuctionHandler : IRequestHandler<StartAuctionCommand, StartAuctionResponse>
    {
        private readonly IAuctionService _auctionService;

        public StartAuctionHandler(IAuctionService auctionService)
        {
            _auctionService = auctionService ?? throw new ArgumentNullException(nameof(auctionService));
        }

        public async Task<StartAuctionResponse> Handle(StartAuctionCommand request, CancellationToken cancellationToken)
        {
            Auction auction = new(
                id: Guid.NewGuid(),
                isActive: true,
                description: request.Description
            );

            var newAuction = await _auctionService.StartAuction(auction, request.VehicleId, cancellationToken);

            return new StartAuctionResponse(
                id: newAuction.Id,
                isActive: newAuction.IsActive,
                description: newAuction.Description
            );
        }
    }
}
