using MediatR;

namespace MyCarAuction.Api.Features.Auctions.Commands.CloseAuction;

internal sealed record CloseAuctionCommand(Guid Id) : IRequest<CloseAuctionResponse>;
