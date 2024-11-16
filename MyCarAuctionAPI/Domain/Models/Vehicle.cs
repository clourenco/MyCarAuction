using MyCarAuctionAPI.Domain.Models.Enums;

namespace MyCarAuctionAPI.Domain.Models
{
    public class Vehicle
    (
        Guid id,
        VehicleType type,
        string manufacturer,
        string model,
        int year,
        int numberOfDoors,
        int? numberOfSeats,
        int? loadCapacity,
        decimal startingBid
    )
    {
        public Guid Id { get; } = id;
        public VehicleType Type { get; } = type;
        public string Manufacturer { get; } = manufacturer;
        public string Model { get; } = model;
        public int Year { get; } = year;
        public int NumberOfDoors { get; } = numberOfDoors;
        public int? NumberOfSeats { get; } = numberOfSeats;
        public int? LoadCapacity { get; } = loadCapacity;
        public decimal StartingBid { get; } = startingBid;
    }
}
