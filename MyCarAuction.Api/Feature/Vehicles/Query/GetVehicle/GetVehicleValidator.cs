using FluentValidation;

namespace MyCarAuction.Api.Features.Vehicles.Queries.GetVehicle;

internal sealed class GetVehicleValidator : AbstractValidator<GetVehicleQuery>
{
    public GetVehicleValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
