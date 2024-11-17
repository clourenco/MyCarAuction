using MediatR;

namespace MyCarAuction.Api.Features.Auctions.Queries.GetAuction;

internal sealed record GetAuctionQuery(Guid Id) : IRequest<GetAuctionResponse>;
