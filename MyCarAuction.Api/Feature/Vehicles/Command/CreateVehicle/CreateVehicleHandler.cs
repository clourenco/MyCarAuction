﻿using MediatR;
using MyCarAuction.Api.Domain.Models;
using MyCarAuction.Api.Domain.Models.Enums;
using MyCarAuction.Api.Features.Vehicles.Interfaces.Services;

namespace MyCarAuction.Api.Features.Vehicles.Commands.CreateVehicle;

internal sealed class CreateVehicleHandler : IRequestHandler<CreateVehicleCommand, CreateVehicleResponse>
{
    private readonly IVehicleService _vehicleService;

    public CreateVehicleHandler(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService ?? throw new ArgumentNullException(nameof(vehicleService));
    }

    public async Task<CreateVehicleResponse> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        Vehicle vehicle = new(
            id: Guid.NewGuid(),
            type: Enum.Parse<VehicleType>(request.Type, ignoreCase: true),
            manufacturer: request.Manufacturer,
            model: request.Model,
            year: request.Year,
            numberOfDoors: request.NumberOfDoors,
            numberOfSeats: request.NumberOfSeats,
            loadCapacity: request.LoadCapacity,
            startingBid: request.StartingBid
        );

        var addedVehicle = await _vehicleService.AddVehicle(vehicle, cancellationToken);

        return new CreateVehicleResponse(
            id: addedVehicle.Id,
            type: addedVehicle.Type.ToString(),
            manufacturer: addedVehicle.Manufacturer,
            model: addedVehicle.Model,
            year: addedVehicle.Year,
            numberOfDoors: addedVehicle.NumberOfDoors,
            numberOfSeats: addedVehicle.NumberOfSeats,
            loadCapacity: addedVehicle.LoadCapacity,
            startingBid: addedVehicle.StartingBid
        );
    }
}
