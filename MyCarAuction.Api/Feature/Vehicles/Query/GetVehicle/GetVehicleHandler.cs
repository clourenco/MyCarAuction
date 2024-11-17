using MediatR;
using MyCarAuction.Api.Features.Vehicles.Interfaces.Services;

namespace MyCarAuction.Api.Features.Vehicles.Queries.GetVehicle;

internal sealed class GetVehicleHandler : IRequestHandler<GetVehicleQuery, GetVehicleResponse>
{
    private readonly IVehicleService _vehicleService;

    public GetVehicleHandler(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService ?? throw new ArgumentNullException(nameof(vehicleService));
    }

    public async Task<GetVehicleResponse> Handle(GetVehicleQuery request, CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleService.GetVehicle(request.Id, cancellationToken);

        return new GetVehicleResponse(
            id: vehicle.Id,
            type: vehicle.Type.ToString(),
            manufacturer: vehicle.Manufacturer,
            model: vehicle.Model,
            year: vehicle.Year,
            numberOfDoors: vehicle.NumberOfDoors,
            numberOfSeats: vehicle.NumberOfSeats,
            loadCapacity: vehicle.LoadCapacity,
            startingBid: vehicle.StartingBid
        );
    }
}
