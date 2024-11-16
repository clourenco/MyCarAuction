using MyCarAuction.Api.Features.Vehicles.Common;

namespace MyCarAuction.Api.Features.Vehicles.Commands.CreateVehicle
{
    public sealed record CreateVehicleResponse : BaseVehicleResponse
    {
        public CreateVehicleResponse(BaseVehicleResponse original) : base(original)
        {
        }

        public CreateVehicleResponse(
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
