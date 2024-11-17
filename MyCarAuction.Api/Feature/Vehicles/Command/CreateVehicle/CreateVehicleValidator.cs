using FluentValidation;
using MyCarAuction.Api.Domain.Models.Enums;

namespace MyCarAuction.Api.Features.Vehicles.Commands.CreateVehicle;

internal sealed class CreateVehicleValidator : AbstractValidator<CreateVehicleCommand>
{
    public CreateVehicleValidator()
    {
        RuleFor(x => x.Type)
            .NotEmpty()
            .Must(type => Enum.TryParse<VehicleType>(type, true, out _))
            .WithMessage("Type must be a valid VehicleType.");

        RuleFor(x => x.Manufacturer).NotEmpty();
        RuleFor(x => x.Model).NotEmpty();

        RuleFor(x => x.Year)
            .InclusiveBetween(1900, DateTime.Now.Year)
            .WithMessage($"Year must be between 1900 and {DateTime.Now.Year}.");

        RuleFor(x => x.NumberOfDoors).GreaterThan(0);

        RuleFor(x => x.StartingBid)
            .InclusiveBetween(1, int.MaxValue)
            .WithMessage($"Starting bid must be between 1 and {int.MaxValue}");

        When(x => x.Type == VehicleType.SUV.ToString(),
            () => RuleFor(x => x.NumberOfSeats)
                    .NotNull()
                    .GreaterThan(0)
                    .InclusiveBetween(1, 9)
                    .WithMessage("Number of seats is required for SUVs and must be between 1 and 9."));

        When(x => x.Type == VehicleType.T.ToString(),
            () => RuleFor(x => x.LoadCapacity)
                    .NotNull()
                    .GreaterThan(0)
                    .InclusiveBetween(500, 500000)
                    .WithMessage("Load capacity is required for Trucks and must be between 500 and 500000 kg."));


    }
}
