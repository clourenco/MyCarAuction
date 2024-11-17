using MyCarAuctionAPI.Domain.Models;
using MyCarAuctionAPI.Domain.Models.Enums;
using MyCarAuctionAPI.Features.Vehicles.Interfaces.Services;
using MyCarAuctionAPI.Infrastructure.Data.Entities;
using MyCarAuction.Api.Features.Vehicles.Repository;
using MyCarAuction.Api.Common.Exceptions;

namespace MyCarAuctionAPI.Features.Vehicles.Services
{
    public sealed class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleService(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
        }

        public async Task<Vehicle> GetVehicle(Guid id, CancellationToken cancellationToken)
        {
            var existentVehicle = await _vehicleRepository.Get(id, cancellationToken);

            if (existentVehicle != null)
                return MapToModel(existentVehicle);
            else
                throw new VehicleNotFoundException($"Vehicle with id {id} was found.");
        }

        public async Task<Vehicle> AddVehicle(Vehicle vehicle, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(vehicle);
            var existentVehicle = await _vehicleRepository.Get(vehicle.Id, cancellationToken);

            if (existentVehicle != null)
                throw new KeyViolationException($"The provided id {vehicle.Id} for the vehicle is already in use.");

            VehicleEntity vehicleEntity = MapToEntity(vehicle);

            var addedVehicle = await _vehicleRepository.Create(vehicleEntity, cancellationToken);

            return MapToModel(addedVehicle);
        }

        public async Task<IEnumerable<Vehicle>> SearchVehicle(
            string? type,
            string? manufacturer,
            string? model,
            int? year,
            CancellationToken cancellationToken
        )
        {
            if (string.IsNullOrWhiteSpace(type)
                && string.IsNullOrWhiteSpace(manufacturer)
                && string.IsNullOrWhiteSpace(model)
                && year == null
            )
            {
                throw new ArgumentException("At least one search criteria must be provided.");
            }

            var vehicleEntities = await _vehicleRepository.FindVehicle(
                type: type,
                manufacturer: manufacturer,
                model: model,
                year: year,
                cancellationToken: cancellationToken
            );

            var vehicleModels = vehicleEntities.Select(e => MapToModel(e));

            return vehicleModels;
        }

        #region Private methods

        private static Vehicle MapToModel(VehicleEntity entity)
        {
            if (!Enum.TryParse<VehicleType>(entity.Type, out VehicleType vehicleType))
                throw new ArgumentException(($"Invalid vehicle type: {entity.Type}"));

            return new(
                id: entity.Id,
                type: vehicleType,
                manufacturer: entity.Manufacturer,
                model: entity.Model,
                year: entity.Year,
                numberOfDoors: entity.NumberOfDoors,
                numberOfSeats: entity.NumberOfSeats,
                loadCapacity: entity.LoadCapacity,
                startingBid: entity.StartingBid
            );
        }

        private static VehicleEntity MapToEntity(Vehicle model)
        {
            return new()
            {
                Id = model.Id,
                Type = model.Type.ToString(),
                Manufacturer = model.Manufacturer,
                Model = model.Model,
                Year = model.Year,
                NumberOfDoors = model.NumberOfDoors,
                NumberOfSeats = model.NumberOfSeats,
                LoadCapacity = model.LoadCapacity,
                StartingBid = model.StartingBid
            };
        }

        #endregion
    }
}
