using FluentValidation;

namespace MyCarAuction.Api.Features.Auctions.Queries.GetAuction;

public sealed class GetAuctionValidator : AbstractValidator<GetAuctionQuery>
{
    public GetAuctionValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
