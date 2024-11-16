using MyCarAuction.Api.Features.Vehicles.Common;

namespace MyCarAuction.Api.Features.Vehicles.Queries.SearchVehicle
{
    public sealed record SearchVehicleResponse : BaseVehicleResponse
    {
        protected SearchVehicleResponse(BaseVehicleResponse original) : base(original)
        {
        }

        public SearchVehicleResponse(
            Guid id,
            string type,
            string manufacturer,
            string model,
            int year,
            int numberOfDoors,
            int? numberOfSeats,
            int? loadCapacity,
            decimal startingBid
        ) : base(
            id,
            type,
            manufacturer,
            model,
            year,
            numberOfDoors,
            numberOfSeats,
            loadCapacity,
            startingBid
        )
        {
        }
    }
}
