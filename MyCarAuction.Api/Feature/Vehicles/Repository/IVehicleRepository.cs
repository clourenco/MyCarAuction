using MyCarAuction.Api.Infrastructure.Data.Entities;
using MyCarAuction.Api.Infrastructure.Repository;

namespace MyCarAuction.Api.Features.Vehicles.Repository;

internal interface IVehicleRepository : IBaseRepository<VehicleEntity>
{
    Task<IEnumerable<VehicleEntity>> FindVehicle(string type, string manufacturer, string model, int? year, CancellationToken cancellationToken);
}
