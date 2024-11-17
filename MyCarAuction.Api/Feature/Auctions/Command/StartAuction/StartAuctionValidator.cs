using FluentValidation;

namespace MyCarAuction.Api.Features.Auctions.Commands.StartAuction;

internal sealed class StartAuctionValidator : AbstractValidator<StartAuctionCommand>
{
    public StartAuctionValidator()
    {
        RuleFor(x => x.VehicleId).NotEmpty();
    }
}
