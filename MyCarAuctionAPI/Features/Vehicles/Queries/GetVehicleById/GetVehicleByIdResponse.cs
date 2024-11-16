using MyCarAuction.Api.Features.Vehicles.Common;

namespace MyCarAuction.Api.Features.Vehicles.Queries.GetVehicleById
{
    public sealed record GetVehicleByIdResponse : BaseVehicleResponse
    {
        public GetVehicleByIdResponse(BaseVehicleResponse original) : base(original)
        {
        }

        public GetVehicleByIdResponse(
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
