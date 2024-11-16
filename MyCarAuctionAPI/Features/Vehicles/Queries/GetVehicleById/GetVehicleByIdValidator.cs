using FluentValidation;

namespace MyCarAuction.Api.Features.Vehicles.Queries.GetVehicleById
{
    public sealed class GetVehicleByIdValidator : AbstractValidator<GetVehicleByIdQuery>
    {
        public GetVehicleByIdValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
