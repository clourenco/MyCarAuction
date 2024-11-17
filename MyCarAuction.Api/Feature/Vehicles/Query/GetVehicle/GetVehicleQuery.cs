using MediatR;

namespace MyCarAuction.Api.Features.Vehicles.Queries.GetVehicle;

internal sealed record GetVehicleQuery(Guid Id) : IRequest<GetVehicleResponse>;
