using FluentValidation;

namespace MyCarAuction.Api.Features.Auctions.Commands.StartAuction;

public sealed class StartAuctionValidator : AbstractValidator<StartAuctionCommand>
{
    public StartAuctionValidator()
    {
        RuleFor(x => x.VehicleId).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
    }
}
