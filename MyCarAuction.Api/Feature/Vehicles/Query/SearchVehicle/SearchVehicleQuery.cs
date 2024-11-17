using MediatR;

namespace MyCarAuction.Api.Features.Vehicles.Queries.SearchVehicle
{
    public sealed record SearchVehicleQuery(
        string? Type,
        string? Manufacturer,
        string? Model,
        int? Year
    ) : IRequest<IEnumerable<SearchVehicleResponse>>;
}
