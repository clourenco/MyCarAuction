using FluentValidation;

namespace MyCarAuction.Api.Features.Bids.Commands.PlaceBid;

public sealed class PlaceBidValidator : AbstractValidator<PlaceBidCommand>
{
    public PlaceBidValidator()
    {
        RuleFor(x => x.AuctionId).NotEmpty();
        RuleFor(x => x.VehicleId).NotEmpty();
        RuleFor(x => x.BidderId).NotEmpty();
        RuleFor(x => x.Amount)
            .InclusiveBetween(1, int.MaxValue)
            .WithMessage($"Starting bid must be between 1 and {int.MaxValue}");
    }
}
