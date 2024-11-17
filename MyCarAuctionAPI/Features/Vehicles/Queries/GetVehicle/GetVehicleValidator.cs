using FluentValidation;

namespace MyCarAuction.Api.Features.Vehicles.Queries.GetVehicle;

public sealed class GetVehicleValidator : AbstractValidator<GetVehicleQuery>
{
    public GetVehicleValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
