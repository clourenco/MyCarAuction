namespace MyCarAuction.Api.Feature.Vehicles.Common.Model;

internal record BaseVehicleResponse(
    Guid Id,
    string Type,
    string Manufacturer,
    string Model,
    int Year,
    int NumberOfDoors,
    int? NumberOfSeats,
    int? LoadCapacity,
    decimal StartingBid
);
