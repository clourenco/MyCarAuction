using MediatR;
using MyCarAuctionAPI.Features.Vehicles.Interfaces.Services;

namespace MyCarAuction.Api.Features.Vehicles.Queries.SearchVehicle
{
    public class SearchVehicleHandler : IRequestHandler<SearchVehicleQuery, IEnumerable<SearchVehicleResponse>>
    {
        private readonly IVehicleService _vehicleService;

        public SearchVehicleHandler(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService ?? throw new ArgumentNullException(nameof(vehicleService));
        }

        public async Task<IEnumerable<SearchVehicleResponse>> Handle(SearchVehicleQuery request, CancellationToken cancellationToken)
        {
            var foundVehicles = await _vehicleService.Search(
                type: request.Type,
                manufacturer: request.Manufacturer,
                model: request.Model,
                year: request.Year,
                cancellationToken: cancellationToken
            );

            return foundVehicles.Select(v => new SearchVehicleResponse(
                    id: v.Id,
                    type: v.Type.ToString(),
                    manufacturer: v.Manufacturer,
                    model: v.Model,
                    year: v.Year,
                    numberOfDoors: v.NumberOfDoors,
                    numberOfSeats: v.NumberOfSeats,
                    loadCapacity: v.LoadCapacity,
                    startingBid: v.StartingBid
                )
            );
        }
    }
}
