using MediatR;
using MyCarAuctionAPI.Features.Vehicles.Interfaces.Services;

namespace MyCarAuction.Api.Features.Vehicles.Queries.GetVehicle
{
    public sealed class GetVehicleHandler : IRequestHandler<GetVehicleQuery, GetVehicleResponse>
    {
        private readonly IVehicleService _vehicleService;

        public GetVehicleHandler(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService ?? throw new ArgumentNullException(nameof(vehicleService));
        }

        public async Task<GetVehicleResponse> Handle(GetVehicleQuery request, CancellationToken cancellationToken)
        {
            var existentVehicle = await _vehicleService.GetVehicle(request.Id, cancellationToken);

            return new GetVehicleResponse(
                id: existentVehicle.Id,
                type: existentVehicle.Type.ToString(),
                manufacturer: existentVehicle.Manufacturer,
                model: existentVehicle.Model,
                year: existentVehicle.Year,
                numberOfDoors: existentVehicle.NumberOfDoors,
                numberOfSeats: existentVehicle.NumberOfSeats,
                loadCapacity: existentVehicle.LoadCapacity,
                startingBid: existentVehicle.StartingBid
            );
        }
    }
}
