using MediatR;

namespace MyCarAuction.Api.Features.Auctions.Commands.StartAuction;

public sealed record StartAuctionCommand(Guid VehicleId, string Description) : IRequest<StartAuctionResponse>;
