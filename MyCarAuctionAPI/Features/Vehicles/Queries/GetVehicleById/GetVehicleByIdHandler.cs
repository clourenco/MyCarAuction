using MediatR;
using MyCarAuctionAPI.Features.Vehicles.Interfaces.Services;

namespace MyCarAuction.Api.Features.Vehicles.Queries.GetVehicleById
{
    public sealed class GetVehicleByIdHandler : IRequestHandler<GetVehicleByIdQuery, GetVehicleByIdResponse>
    {
        private readonly IVehicleService _vehicleService;

        public GetVehicleByIdHandler(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService ?? throw new ArgumentNullException(nameof(vehicleService));
        }

        public async Task<GetVehicleByIdResponse> Handle(GetVehicleByIdQuery request, CancellationToken cancellationToken)
        {
            var existentVehicle = await _vehicleService.GetById(request.Id, cancellationToken);

            return new GetVehicleByIdResponse(
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
