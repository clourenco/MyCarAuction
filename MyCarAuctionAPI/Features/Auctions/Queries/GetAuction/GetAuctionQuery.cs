using MediatR;

namespace MyCarAuction.Api.Features.Auctions.Queries.GetAuction;

public sealed record GetAuctionQuery(Guid Id) : IRequest<GetAuctionResponse>;
