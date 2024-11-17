using MediatR;

namespace MyCarAuction.Api.Features.Auctions.Commands.CloseAuction;

public sealed record CloseAuctionCommand(Guid Id) : IRequest<CloseAuctionResponse>;
