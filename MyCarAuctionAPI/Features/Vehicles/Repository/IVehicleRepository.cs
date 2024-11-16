using MyCarAuctionAPI.Infrastructure.Data.Entities;
using MyCarAuctionAPI.Infrastructure.Repository;

namespace MyCarAuction.Api.Features.Vehicles.Repository
{
    public interface IVehicleRepository : IBaseRepository<VehicleEntity>
    {
        Task<IEnumerable<VehicleEntity>> Find(string? type, string? manufacturer, string? model, int? year, CancellationToken cancellationToken);
    }
}
