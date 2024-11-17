using MediatR;

namespace MyCarAuction.Api.Features.Auctions.Commands.StartAuction;

internal sealed record StartAuctionCommand(Guid VehicleId, string Description) : IRequest<StartAuctionResponse>;
