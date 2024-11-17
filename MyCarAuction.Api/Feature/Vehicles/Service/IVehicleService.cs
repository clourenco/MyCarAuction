using MyCarAuction.Api.Domain.Models;

namespace MyCarAuction.Api.Features.Vehicles.Interfaces.Services;

internal interface IVehicleService
{
    Task<Vehicle> GetVehicle(Guid id, CancellationToken cancellationToken);
    Task<Vehicle> AddVehicle(Vehicle vehicle, CancellationToken cancellationToken);
    Task<IEnumerable<Vehicle>> SearchVehicle(string type, string manufacturer, string model, int? year, CancellationToken cancellationToken);
}
