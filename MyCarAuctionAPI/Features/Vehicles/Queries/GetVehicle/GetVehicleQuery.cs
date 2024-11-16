using MediatR;

namespace MyCarAuction.Api.Features.Vehicles.Queries.GetVehicle
{
    public sealed record GetVehicleQuery(Guid Id) : IRequest<GetVehicleResponse>;
}
