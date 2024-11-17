using FluentValidation;

namespace MyCarAuction.Api.Features.Vehicles.Queries.SearchVehicle
{
    public sealed class SearchVehicleValidator : AbstractValidator<SearchVehicleQuery>
    {
        public SearchVehicleValidator()
        {
            RuleFor(x => x)
                .Must(HaveAtLeastOneNonEmptyField)
                .WithMessage("At least one field (Type, Manufacturer, Model, Year) must be provided.");
        }

        private bool HaveAtLeastOneNonEmptyField(SearchVehicleQuery query)
        {
            return !string.IsNullOrEmpty(query.Type) ||
                   !string.IsNullOrEmpty(query.Manufacturer) ||
                   !string.IsNullOrEmpty(query.Model) ||
                   query.Year.HasValue;
        }
    }
}
