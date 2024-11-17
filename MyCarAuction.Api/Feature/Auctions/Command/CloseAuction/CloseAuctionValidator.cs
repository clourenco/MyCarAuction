using FluentValidation;

namespace MyCarAuction.Api.Features.Auctions.Commands.CloseAuction;

internal sealed class CloseAuctionValidator : AbstractValidator<CloseAuctionCommand>
{
    public CloseAuctionValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
