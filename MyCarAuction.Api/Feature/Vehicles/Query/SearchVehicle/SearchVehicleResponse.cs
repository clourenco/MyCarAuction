using MyCarAuction.Api.Feature.Vehicles.Common.Model;

namespace MyCarAuction.Api.Features.Vehicles.Queries.SearchVehicle;

internal sealed record SearchVehicleResponse : BaseVehicleResponse
{
    public SearchVehicleResponse(BaseVehicleResponse original) : base(original)
    {
    }

    public SearchVehicleResponse(
        Guid id,
        string type,
        string manufacturer,
        string model,
        int year,
        int numberOfDoors,
        int? numberOfSeats,
        int? loadCapacity,
        decimal startingBid
    ) : base(
        id,
        type,
        manufacturer,
        model,
        year,
        numberOfDoors,
        numberOfSeats,
        loadCapacity,
        startingBid
    )
    {
    }
}
