using MyCarAuction.Api.Features.Vehicles.Common;

namespace MyCarAuction.Api.Features.Vehicles.Queries.GetVehicle
{
    public sealed record GetVehicleResponse : BaseVehicleResponse
    {
        public GetVehicleResponse(BaseVehicleResponse original) : base(original)
        {
        }

        public GetVehicleResponse(
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
