using MediatR;

namespace MyCarAuction.Api.Features.Vehicles.Commands.CreateVehicle;

internal sealed record CreateVehicleCommand(
    string Type,
    string Manufacturer,
    string Model,
    int Year,
    int NumberOfDoors,
    int? NumberOfSeats,
    int? LoadCapacity,
    decimal StartingBid
) : IRequest<CreateVehicleResponse>;
