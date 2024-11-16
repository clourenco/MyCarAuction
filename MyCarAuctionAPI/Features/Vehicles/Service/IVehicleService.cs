using MyCarAuctionAPI.Domain.Models;

namespace MyCarAuctionAPI.Features.Vehicles.Interfaces.Services
{
    public interface IVehicleService
    {
        Task<Vehicle> GetById(Guid id, CancellationToken cancellationToken);
        Task<Vehicle> Add(Vehicle vehicle, CancellationToken cancellationToken);
        Task<IEnumerable<Vehicle>> Search(string? type, string? manufacturer, string? model, int? year, CancellationToken cancellationToken);
    }
}
