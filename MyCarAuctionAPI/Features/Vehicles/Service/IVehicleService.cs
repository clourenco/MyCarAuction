using MyCarAuctionAPI.Domain.Models;

namespace MyCarAuctionAPI.Features.Vehicles.Interfaces.Services
{
    public interface IVehicleService
    {
        Task<Vehicle> GetVehicle(Guid id, CancellationToken cancellationToken);
        Task<Vehicle> AddVehicle(Vehicle vehicle, CancellationToken cancellationToken);
        Task<IEnumerable<Vehicle>> SearchVehicle(string? type, string? manufacturer, string? model, int? year, CancellationToken cancellationToken);
    }
}
