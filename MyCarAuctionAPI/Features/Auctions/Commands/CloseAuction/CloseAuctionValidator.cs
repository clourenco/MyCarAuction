using FluentValidation;

namespace MyCarAuction.Api.Features.Auctions.Commands.CloseAuction;

public sealed class CloseAuctionValidator : AbstractValidator<CloseAuctionCommand>
{
    public CloseAuctionValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
